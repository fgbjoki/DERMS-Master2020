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
        private static readonly NetTcpBinding netTcpBinding = new NetTcpBinding();

        private Dictionary<string, WCFClient<ITransaction>> servicesInTransaction;

        private ReaderWriterLockSlim transactionStateLocker;

        private TransactionStateWrapper transactionStateWrapper;

        private TransactionPhaseExecutor phaseExecutor;

        public TransactionProcessor()
        {
            transactionStateWrapper = new TransactionStateWrapper(new TransactionIdleState());
            transactionStateLocker = new ReaderWriterLockSlim();

            phaseExecutor = new TransactionPhaseExecutor(transactionStateLocker);

            servicesInTransaction = new Dictionary<string, WCFClient<ITransaction>>();
        }

        /// <inheritdoc/>
        public bool StartEnlist()
        {
            bool isCommandSuccessful = true;

            try
            {
                transactionStateLocker.EnterWriteLock();

                transactionStateWrapper.CurrentState = transactionStateWrapper.CurrentState.StartEnlist();

                transactionStateLocker.ExitWriteLock();

                Logger.Instance.Log("Start enlist successfully called.");
            }
            catch (ApplicationException ae)
            {
                Logger.Instance.Log(ae.Message);
                Logger.Instance.Log("Start enlist failed due to timeout while waiting for reader lock.");

                if (transactionStateLocker.IsWriteLockHeld)
                {
                    transactionStateLocker.ExitWriteLock();
                }

                isCommandSuccessful = false;
            }
            catch (TransactionException te)
            {
                Logger.Instance.Log(te.Message);

                if (transactionStateLocker.IsWriteLockHeld)
                {
                    transactionStateLocker.ExitWriteLock();
                }

                isCommandSuccessful = false;
            }
            catch (Exception e)
            {
                Logger.Instance.Log(e.Message);

                if (transactionStateLocker.IsWriteLockHeld)
                {
                    transactionStateLocker.ExitWriteLock();
                }

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

                transactionStateLocker.EnterWriteLock();

                transactionStateWrapper.CurrentState = transactionStateWrapper.CurrentState.Enlist(serviceName);
                servicesInTransaction.Add(serviceName, wcfClient);

                transactionStateLocker.ExitWriteLock();

                Logger.Instance.Log($"\"{serviceName}\" enlisted.");

            }
            catch (ApplicationException ae)
            {
                Logger.Instance.Log(ae.Message);
                Logger.Instance.Log("Enlist service failed due to timeout while waiting for reader lock.");

                if (transactionStateLocker.IsWriteLockHeld)
                {
                    transactionStateLocker.ExitWriteLock();
                }

                isCommandSuccessful = false;
            }
            catch (TransactionException te)
            {
                Logger.Instance.Log(te.Message);

                if (transactionStateLocker.IsWriteLockHeld)
                {
                    transactionStateLocker.ExitWriteLock();
                }

                isCommandSuccessful = false;
            }
            catch (Exception e)
            {
                Logger.Instance.Log(e.Message);

                if (transactionStateLocker.IsWriteLockHeld)
                {
                    transactionStateLocker.ExitWriteLock();
                }

                isCommandSuccessful = false;
            }

            if (!isCommandSuccessful)
            {
                Logger.Instance.Log($"\"{serviceName}\" couldn't enlist.");
            }

            return isCommandSuccessful;
        }

        /// <inheritdoc/>
        public bool EndEnlist(bool allServicesEnlisted)
        {
            bool isCommandSuccessful = true;

            try
            {
                transactionStateLocker.EnterWriteLock();

                transactionStateWrapper.CurrentState = transactionStateWrapper.CurrentState.EndEnlist(allServicesEnlisted);

                transactionStateLocker.ExitWriteLock();

                Logger.Instance.Log($"EndEnlist called with {allServicesEnlisted == true}.");

                if (allServicesEnlisted)
                {
                    Logger.Instance.Log($"EndEnlist going into Prepare phase.");
                    phaseExecutor.SchedulePreparePhase(servicesInTransaction, transactionStateWrapper);
                }
                else
                {
                    Logger.Instance.Log($"EndEnlist going into Rollback phase.");
                    phaseExecutor.ScheduleRollbackPhase(servicesInTransaction, transactionStateWrapper);
                }
            }
            catch (ApplicationException ae)
            {
                Logger.Instance.Log(ae.Message);
                Logger.Instance.Log("End enlist failed due to timeout while waiting for reader lock.");

                if (transactionStateLocker.IsWriteLockHeld)
                {
                    transactionStateLocker.ExitWriteLock();
                }

                isCommandSuccessful = false;
            }
            catch (TransactionException te)
            {
                Logger.Instance.Log(te.Message);

                if (transactionStateLocker.IsWriteLockHeld)
                {
                    transactionStateLocker.ExitWriteLock();
                }

                isCommandSuccessful = false;
            }
            catch (Exception e)
            {
                Logger.Instance.Log(e.Message);

                if (transactionStateLocker.IsWriteLockHeld)
                {
                    transactionStateLocker.ExitWriteLock();
                }

                isCommandSuccessful = false;
            }

            return isCommandSuccessful;
        }
    }
}
