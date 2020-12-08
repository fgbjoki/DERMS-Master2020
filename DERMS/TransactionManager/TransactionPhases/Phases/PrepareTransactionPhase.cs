using System.Threading;
using Common.ServiceInterfaces.Transaction;
using TransactionManager.TransactionStates;
using System.Collections.Generic;
using Common.Logger;

namespace TransactionManager.TransactionPhases
{
    internal sealed class PrepareTransactionPhase : TransactionPhase
    {
        private Dictionary<string, ITransactionCallback> preparedServices;
        private Dictionary<string, ITransactionCallback> enlistedServices;

        public PrepareTransactionPhase(ReaderWriterLock transactionStateLocker,
                                        ReaderWriterLock phaseLocker,
                                        TransactionStateWrapper transactionStateWrapper, 
                                        Semaphore semaphore,
                                        Dictionary<string, ITransactionCallback> services) : base(transactionStateLocker, phaseLocker, transactionStateWrapper, semaphore, services)
        {
            preparedServices = new Dictionary<string, ITransactionCallback>(services.Count);
            enlistedServices = services;
        }

        /// <summary>
        /// Depending on the outcome of the prepare phase returns either <see cref="TransactionRollbackState"/> or <see cref="CommitTransactionPhase"/>.
        /// </summary>
        /// <param name="isTransactionSuccessful">Indicates if the prepare phase was successful.</param>
        /// <returns>Returns <see cref="CommitTransactionPhase"/>if the prepare phase was successful, otherwise <see cref="RollbackTransactionPhase"/>.</returns>
        protected override TransactionPhase GetNextPhase(bool isTransactionSuccessful)
        {
            if (isTransactionSuccessful)
            {
                return new CommitTransactionPhase(transactionStateLocker, phaseLocker, transactionStateWrapper, semaphore, preparedServices);
            }
            else
            {
                enlistedServices.Clear();
                return new RollbackTransactionPhase(transactionStateLocker, phaseLocker, transactionStateWrapper, semaphore, preparedServices);
            }
        }

        /// <summary>
        /// Executes <see cref="ITransactionCallback.Prepare"/> on each transaction participant.
        /// </summary>
        protected override bool ExecutePhaseFunction(string serviceName, ITransactionCallback serviceCallback)
        {
            bool isFunctionSuccessful = true;

            try
            {
                DERMSLogger.Instance.Log($"[ExecutePhases] Executing prepare phase on \"{serviceName}\".");
                isFunctionSuccessful = serviceCallback.Prepare();
                preparedServices.Add(serviceName, serviceCallback);
            }
            catch
            {
                isFunctionSuccessful = false;
            }

            string messageAddition = isFunctionSuccessful == true ? "successful" : "unsuccessful";
            DERMSLogger.Instance.Log($"[ExecutePhases] Prepare phase on \"{serviceName}\" was {messageAddition}.");

            return isFunctionSuccessful;
        }

        /// <inheritdoc/>
        protected override TransactionState ChangeTransactionState(string serviceName)
        {
            return transactionStateWrapper.CurrentState.Prepare(serviceName);
        }
    }
}
