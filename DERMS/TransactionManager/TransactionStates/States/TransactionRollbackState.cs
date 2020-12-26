using System.Collections.Generic;

namespace TransactionManager.TransactionStates
{
    /// <inheritdoc/>
    sealed class TransactionRollbackState : TransactionState
    {
        private HashSet<string> preparedServices;

        internal TransactionRollbackState(HashSet<string> preparedServices) : base(TransactionStateEnum.Rollback)
        {
            this.preparedServices = preparedServices;
        }

        public override TransactionState Commit(string serviceName)
        {
            throw new TransactionException(State, TransactionStateEnum.Commit);
        }

        public override TransactionState EndEnlist(bool successful)
        {
            throw new TransactionException(State, TransactionStateEnum.Rollback);
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
            if (!preparedServices.Remove(serviceName))
            {
                throw new TransactionException(State, State, $"Service \"{serviceName}\" wasn't prepared.");
            }

            if (preparedServices.Count == 0)
            {
                return new TransactionIdleState();
            }

            return this;
        }

        public override TransactionState StartEnlist()
        {
            throw new TransactionException(State, TransactionStateEnum.Enlist);
        }
    }
}
