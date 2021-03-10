using System.Collections.Generic;

namespace TransactionManager.TransactionStates
{
    /// <inheritdoc/>
    sealed class TransactionCommitState : TransactionState
    {
        private HashSet<string> commitedServices;
        private HashSet<string> enlisedServices;

        internal TransactionCommitState(HashSet<string> services) : base(TransactionStateEnum.Commit)
        {
            this.enlisedServices = services;
            commitedServices = new HashSet<string>();
        }

        public override TransactionState Commit(string serviceName)
        {
            if (!commitedServices.Add(serviceName))
            {
                throw new TransactionException(State, State, $"Service \"{serviceName}\" is already commited.");
            }

            if (commitedServices.Count == enlisedServices.Count)
            {
                return new TransactionIdleState();
            }

            return this;
        }

        public override TransactionState EndEnlist(bool successful)
        {
            if (successful)
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
            throw new TransactionException(State, TransactionStateEnum.Prepare);
        }

        public override TransactionState Rollback(string serviceName)
        {
            // TODO, pitaj Nedica
            throw new TransactionException(State, TransactionStateEnum.Rollback);
        }

        public override TransactionState StartEnlist()
        {
            throw new TransactionException(State, TransactionStateEnum.Enlist);
        }
    }
}
