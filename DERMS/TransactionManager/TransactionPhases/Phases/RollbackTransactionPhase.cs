﻿using System.Threading;
using Common.ServiceInterfaces.Transaction;
using TransactionManager.TransactionStates;
using System.Collections.Generic;
using Common.Logger;
using Common.Communication;

namespace TransactionManager.TransactionPhases
{
    /// <summary>
    /// Represents the rollback transaction phase.
    /// </summary>
    class RollbackTransactionPhase : TransactionPhase
    {
        public RollbackTransactionPhase(ReaderWriterLockSlim transactionStateLocker, ReaderWriterLock phaseLocker, TransactionStateWrapper transactionStateWrapper,
                                        Dictionary<string, WCFClient<ITransaction>> services) : base(transactionStateLocker, phaseLocker, transactionStateWrapper, services)
        {

        }

        /// <inheritdoc/>
        protected override TransactionState ChangeTransactionState(string serviceName)
        {
            return transactionStateWrapper.CurrentState.Rollback(serviceName);
        }

        /// <summary>
        /// Executes <see cref="ITransaction.Rollback"/> on each transaction participant.
        /// </summary>
        protected override bool ExecutePhaseFunction(string serviceName, WCFClient<ITransaction> serviceClient)
        {
            bool isFunctionSuccessful = true;

            try
            {
                Logger.Instance.Log($"[ExecutePhases] Executing rollback phase on \"{serviceName}\".");

                isFunctionSuccessful = serviceClient.Proxy.Rollback();
            }
            catch
            {
                isFunctionSuccessful = false;
            }


            string messageAddition = isFunctionSuccessful == true ? "successful" : "unsuccessful";
            Logger.Instance.Log($"[ExecutePhases] Rollback phase on \"{serviceName}\" was {messageAddition}.");

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
