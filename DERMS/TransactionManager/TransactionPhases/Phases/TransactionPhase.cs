using Common.Communication;
using Common.Logger;
using Common.ServiceInterfaces.Transaction;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TransactionManager.TransactionStates;

namespace TransactionManager.TransactionPhases
{
    /// <summary>
    /// Defines common structure of each distributed transaction phase.
    /// </summary>
    internal abstract class TransactionPhase
    {
        private static readonly int LOCKER_TIME_OUT = 120000; /// 120 seconds
        protected TransactionPhase nextPhase;

        protected ReaderWriterLock transactionStateLocker;
        protected ReaderWriterLock phaseLocker;

        protected TransactionStateWrapper transactionStateWrapper;

        protected Semaphore semaphore;

        private readonly Dictionary<string, WCFClient<ITransaction>> services;

        protected TransactionPhase(ReaderWriterLock transactionStateLocker, ReaderWriterLock phaseLocker, TransactionStateWrapper transactionStateWrapper, Semaphore semaphore, Dictionary<string, WCFClient<ITransaction>> services)
        {
            this.transactionStateLocker = transactionStateLocker;
            this.phaseLocker = phaseLocker;
            this.transactionStateWrapper = transactionStateWrapper;
            this.semaphore = semaphore;
            this.services = services;
        }

        /// <summary>
        /// Executes current phase concurrently.
        /// </summary>
        public void Execute()
        {
            bool isSuccessful = true;
            List<Task<bool>> tasks = new List<Task<bool>>(services.Count);

            try
            {
                phaseLocker.AcquireReaderLock(LOCKER_TIME_OUT);

                foreach (KeyValuePair<string, WCFClient<ITransaction>> serviceCallback in services)
                {
                    // log service name
                    Task<bool> callServiceThread = new Task<bool>(() => ExecuteAction(serviceCallback.Key, serviceCallback.Value));

                    callServiceThread.Start();

                    tasks.Add(callServiceThread);
                }

                phaseLocker.ReleaseReaderLock();
            }
            catch (ApplicationException ae)
            {
                DERMSLogger.Instance.Log($"[ExecutePhase] Transaction phase {this.GetType().ToString()} failed due to locker time out.");

                return;
            }
            catch (Exception e)
            {
                phaseLocker.ReleaseReaderLock();

                return;
            }

            Task.WaitAll(tasks.ToArray());

            tasks.ForEach(task => isSuccessful &= task.Result);

            nextPhase = GetNextPhase(isSuccessful);
        }

        public TransactionPhase NextPhase
        {
            get
            {
                return nextPhase;
            }
        }

        /// <summary>
        /// Depending on the current phase and the outcome of the phase, next <see cref="TransactionPhase"/> will be returned.
        /// </summary>
        /// <param name="isTransactionSuccessful">Indicates is the current phase successfuly executed.</param>
        /// <returns>Next <see cref="TransactionPhase"/>.</returns>
        protected abstract TransactionPhase GetNextPhase(bool isTransactionSuccessful);

        /// <summary>
        /// Executes one of <see cref="ITransactionCallback"/> function, it depends which phase is currently executing. 
        /// </summary>
        /// <param name="serviceName">Name of the the current processing service.</param>
        /// <param name="serviceClient"><see cref="WCFClient{T}"/> for the <paramref name="serviceName"/> service.</param>
        /// <returns><b>True</b> if the specific <see cref="ITransactionCallback"/> is successful, otherwise <b>false</b>.</returns>
        protected abstract bool ExecutePhaseFunction(string serviceName, WCFClient<ITransaction> serviceClient);

        /// <summary>
        /// Changes the current state of the transaction based on which phase is executing.
        /// </summary>
        /// <param name="serviceName">Service name in which is currently processed.</param>
        /// <returns>Next <see cref="TransactionState"/>.</returns>
        protected abstract TransactionState ChangeTransactionState(string serviceName);

        private bool ExecuteAction(string serviceName, WCFClient<ITransaction> clientService)
        {
            bool successfulyExecuted = true;

            successfulyExecuted = ExecutePhaseFunction(serviceName, clientService);

            try
            {
                transactionStateLocker.AcquireWriterLock(LOCKER_TIME_OUT);

                if (successfulyExecuted)
                {
                    transactionStateWrapper.CurrentState = ChangeTransactionState(serviceName);
                    services.Remove(serviceName);
                }

                transactionStateLocker.ReleaseWriterLock();
            }
            catch (ApplicationException ae)
            {
                DERMSLogger.Instance.Log($"[ExecutePhase] Transaction phase {this.GetType().ToString()} failed due to locker time out.");

                successfulyExecuted = false;
            }
            catch (TransactionException te)
            {
                transactionStateLocker.ReleaseWriterLock();

                DERMSLogger.Instance.Log(te.Message);

                successfulyExecuted = false;
            }
            catch (Exception e)
            {
                transactionStateLocker.ReleaseWriterLock();

                DERMSLogger.Instance.Log(e.Message);

                successfulyExecuted = false;
            }

            return successfulyExecuted;
        }
    }
}
