using System.Collections.Generic;
using System.Threading;
using Common.ServiceInterfaces.Transaction;
using TransactionManager.TransactionStates;
using Common.Logger;
using Common.Communication;

namespace TransactionManager.TransactionPhases
{
    /// <summary>
    /// Represents the commit transaction phase.
    /// </summary>
    class CommitTransactionPhase : TransactionPhase
    {
        public CommitTransactionPhase(ReaderWriterLockSlim transactionStateLocker, ReaderWriterLock phaseLocker, TransactionStateWrapper transactionStateWrapper, Semaphore semaphore,
                                      Dictionary<string, WCFClient<ITransaction>> services) : base(transactionStateLocker, phaseLocker, transactionStateWrapper, semaphore, services)
        {
        }

        /// <inheritdoc/>
        protected override TransactionState ChangeTransactionState(string serviceName)
        {
            return  transactionStateWrapper.CurrentState.Commit(serviceName);
        }

        /// <summary>
        /// Executes <see cref="ITransaction.Commit"/> on each transaction participant.
        /// </summary>
        protected override bool ExecutePhaseFunction(string serviceName, WCFClient<ITransaction> serviceClient)
        {
            bool isFunctionSuccessful = true;

            try
            {
                Logger.Instance.Log($"[ExecutePhases] Executing commit phase on \"{serviceName}\".");
                isFunctionSuccessful = serviceClient.Proxy.Commit();
            }
            catch
            {
                isFunctionSuccessful = false;
            }

            string messageAddition = isFunctionSuccessful == true ? "successful" : "unsuccessful";
            Logger.Instance.Log($"[ExecutePhases] Commit phase on \"{serviceName}\" was {messageAddition}.");

            return isFunctionSuccessful;
        }

        /// <summary>
        /// Rollback doesn't have next phase.
        /// </summary>
        protected override TransactionPhase GetNextPhase(bool isTransactionSuccessful)
        {
            return null;
        }
    }
}
