using Common.Communication;
using Common.Logger;
using Common.ServiceInterfaces.Transaction;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;
using TransactionManager.TransactionPhases;
using TransactionManager.TransactionStates;

namespace TransactionManager
{
    /// <summary>
    /// Component resposible for scheduling each distributed transaction command 
    /// </summary>
    internal class TransactionProcessor : ITransactionManager
    {
        private static readonly int TIME_OUT_PERIOD = 10000;

        private static readonly NetTcpBinding netTcpBinding = new NetTcpBinding();

        private Dictionary<string, WCFClient<ITransaction>> servicesInTransaction;

        private ReaderWriterLock transactionStateLocker;

        private TransactionStateWrapper transactionStateWrapper;

        private TransactionPhaseExecutor phaseExecutor;

        public TransactionProcessor()
        {
            transactionStateWrapper = new TransactionStateWrapper(new TransactionIdleState());
            transactionStateLocker = new ReaderWriterLock();

            phaseExecutor = new TransactionPhaseExecutor(transactionStateLocker);

            servicesInTransaction = new Dictionary<string, WCFClient<ITransaction>>();
        }

        /// <inheritdoc/>
        public bool StartEnlist()
        {
            bool isCommandSuccessful = true;
            try
            {
                transactionStateLocker.AcquireWriterLock(TIME_OUT_PERIOD);

                transactionStateWrapper.CurrentState = transactionStateWrapper.CurrentState.StartEnlist();
                DERMSLogger.Instance.Log("Start enlist successfuly called.");

                transactionStateLocker.ReleaseWriterLock();
            }
            catch (ApplicationException ae)
            {
                DERMSLogger.Instance.Log("Start enlist failed due to timeout while waiting for reader lock.");

                transactionStateLocker.ReleaseWriterLock();
                isCommandSuccessful = false;
            }
            catch (TransactionException te)
            {
                DERMSLogger.Instance.Log(te.Message);

                transactionStateLocker.ReleaseWriterLock();
                isCommandSuccessful = false;
            }
            catch (Exception e)
            {
                DERMSLogger.Instance.Log(e.Message);

                transactionStateLocker.ReleaseWriterLock();
                isCommandSuccessful = false;
            }

            return isCommandSuccessful;
        }

        /// <inheritdoc/>
        public bool EnlistService(string serviceName, string endpointAddress)
        {
            bool isCommandSuccessful = true;

            try
            {
                var wcfClient = new WCFClient<ITransaction>(netTcpBinding, new EndpointAddress(endpointAddress));

                transactionStateLocker.AcquireWriterLock(TIME_OUT_PERIOD);

                transactionStateWrapper.CurrentState = transactionStateWrapper.CurrentState.Enlist(serviceName);

                servicesInTransaction.Add(serviceName, wcfClient);

                transactionStateLocker.ReleaseWriterLock();

                DERMSLogger.Instance.Log($"\"{serviceName}\" enlisted.");

            }
            catch (ApplicationException ae)
            {
                DERMSLogger.Instance.Log("Enlist service failed due to timeout while waiting for reader lock.");

                isCommandSuccessful = false;
            }
            catch (TransactionException te)
            {
                DERMSLogger.Instance.Log(te.Message);

                transactionStateLocker.ReleaseWriterLock();
                isCommandSuccessful = false;
            }
            catch (Exception e)
            {
                DERMSLogger.Instance.Log(e.Message);

                transactionStateLocker.ReleaseWriterLock();
                isCommandSuccessful = false;
            }

            if (!isCommandSuccessful)
            {
                DERMSLogger.Instance.Log($"\"{serviceName}\" couldn't enlist.");
            }

            return isCommandSuccessful;
        }

        /// <inheritdoc/>
        public bool EndEnlist(bool allServicesEnlisted)
        {
            bool isCommandSuccessful = true;
            try
            {
                transactionStateLocker.AcquireWriterLock(TIME_OUT_PERIOD);

                transactionStateWrapper.CurrentState = transactionStateWrapper.CurrentState.EndEnlist(allServicesEnlisted);

                transactionStateLocker.ReleaseWriterLock();

                DERMSLogger.Instance.Log($"EndEnlist called with {allServicesEnlisted == true}.");

                if (allServicesEnlisted)
                {
                    DERMSLogger.Instance.Log($"EndEnlist going into Prepare phase.");
                    phaseExecutor.SchedulePreparePhase(servicesInTransaction, transactionStateWrapper);
                }
                else
                {
                    DERMSLogger.Instance.Log($"EndEnlist going into Rollback phase.");
                    phaseExecutor.ScheduleRollbackPhase(servicesInTransaction, transactionStateWrapper);
                }
            }
            catch (ApplicationException ae)
            {
                DERMSLogger.Instance.Log("End enlist failed due to timeout while waiting for reader lock.");

                isCommandSuccessful = false;
            }
            catch (TransactionException te)
            {
                DERMSLogger.Instance.Log(te.Message);

                transactionStateLocker.ReleaseWriterLock();
                isCommandSuccessful = false;
            }
            catch (Exception e)
            {
                DERMSLogger.Instance.Log(e.Message);

                transactionStateLocker.ReleaseWriterLock();
                isCommandSuccessful = false;
            }

            return isCommandSuccessful;
        }
    }
}
