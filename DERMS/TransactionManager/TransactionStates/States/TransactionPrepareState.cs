using Common.Logger;
using System.Collections.Generic;

namespace TransactionManager.TransactionStates
{
    /// <inheritdoc/>
    sealed class TransactionPrepareState : TransactionState
    {
        private HashSet<string> preparedServices;
        private HashSet<string> enlistedServices;

        internal TransactionPrepareState(HashSet<string> enlistedServices) : base(TransactionStateEnum.Prepare)
        {
            this.enlistedServices = enlistedServices;
            preparedServices = new HashSet<string>(this.enlistedServices.Count);
        }

        public override TransactionState Commit(string serviceName)
        {
            if (preparedServices.Count != enlistedServices.Count)
            {
                throw new TransactionException(State, TransactionStateEnum.Commit, "Not all services are prepared.");
            }

            foreach (string preparedService in preparedServices)
            {
                if (!enlistedServices.Contains(preparedService))
                {
                    throw new TransactionException(State, TransactionStateEnum.Commit, "Not all services are prepared.");
                }
            }
            
            return new TransactionCommitState(preparedServices).Commit(serviceName);
        }

        public override TransactionState EndEnlist(bool successful)
        {
            if(successful)
            {
                throw new TransactionException(State, TransactionStateEnum.Prepare);
            }
            else
            {
                throw new TransactionException(State, TransactionStateEnum.Rollback);
            }
        }

        public override TransactionState Enlist(string serviceName)
        {
            throw new TransactionException(State, TransactionStateEnum.Enlist);
        }

        public override TransactionState Prepare(string serviceName)
        {
            if (!preparedServices.Add(serviceName) && enlistedServices.Contains(serviceName))
            {
                throw new TransactionException(State, State, $"Service \"{serviceName}\" is already prepared.");
            }

            return this;
        }

        public override TransactionState Rollback(string serviceName)
        {
            if (enlistedServices.Count == 0 && preparedServices.Count == 0)
            {
                // log no rollback needed.

                return new TransactionIdleState();
            }
            
            return new TransactionRollbackState(enlistedServices).Rollback(serviceName);
        }

        public override TransactionState StartEnlist()
        {
            throw new TransactionException(State, TransactionStateEnum.Enlist);
        }
    }
}
