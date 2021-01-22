using System.Threading;
using Common.ServiceInterfaces.Transaction;
using TransactionManager.TransactionStates;
using System.Collections.Generic;
using Common.Logger;
using Common.Communication;
using System.Linq;

namespace TransactionManager.TransactionPhases
{
    internal sealed class PrepareTransactionPhase : TransactionPhase
    {
        private Dictionary<string, WCFClient<ITransaction>> preparedServices;
        private Dictionary<string, WCFClient<ITransaction>> enlistedServices;

        public PrepareTransactionPhase(ReaderWriterLockSlim transactionStateLocker,
                                        ReaderWriterLock phaseLocker,
                                        TransactionStateWrapper transactionStateWrapper, 
                                        Semaphore semaphore,
                                        Dictionary<string, WCFClient<ITransaction>> services) : base(transactionStateLocker, phaseLocker, transactionStateWrapper, semaphore, services)
        {
            preparedServices = new Dictionary<string, WCFClient<ITransaction>>(services.Count);
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
                Dictionary<string, WCFClient<ITransaction>> servicesToRollBack = preparedServices;

                foreach (var enlistService in enlistedServices)
                {
                    servicesToRollBack.Add(enlistService.Key, enlistService.Value);
                }

                return new RollbackTransactionPhase(transactionStateLocker, phaseLocker, transactionStateWrapper, semaphore, servicesToRollBack);
            }
        }

        /// <summary>
        /// Executes <see cref="ITransaction.Prepare"/> on each transaction participant.
        /// </summary>
        protected override bool ExecutePhaseFunction(string serviceName, WCFClient<ITransaction> serviceClient)
        {
            bool isFunctionSuccessful = true;

            try
            {
                DERMSLogger.Instance.Log($"[ExecutePhases] Executing prepare phase on \"{serviceName}\".");

                isFunctionSuccessful = serviceClient.Proxy.Prepare();

                preparedServices.Add(serviceName, serviceClient);

                enlistedServices.Remove(serviceName);
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
