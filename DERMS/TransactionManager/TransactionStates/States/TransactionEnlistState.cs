using System.Collections.Generic;

namespace TransactionManager.TransactionStates
{
    /// <inheritdoc/>
    sealed class TransactionEnlistState : TransactionState
    {
        private readonly bool isEnlistFinished;
        private HashSet<string> enlistedServices;

        internal TransactionEnlistState(bool enlistFinished) : base(TransactionStateEnum.Enlist)
        {
            if (isEnlistFinished == false && enlistedServices == null)
            {
                enlistedServices = new HashSet<string>();
            }

            isEnlistFinished = enlistFinished;
        }

        public override TransactionState Commit(string serviceName)
        {
            throw new TransactionException(State, TransactionStateEnum.Commit);
        }

        public override TransactionState EndEnlist(bool successful)
        {
            if (isEnlistFinished)
            {
                throw new TransactionException(State, TransactionStateEnum.Enlist, "End enlist already called");
            }

            // EndEnlist called with false which means something went wrong and transaction needs to rollback.
            if (!successful)
            {
                return new TransactionRollbackState(enlistedServices);
            }

            TransactionEnlistState newEnlistState = new TransactionEnlistState(true);
            newEnlistState.enlistedServices = this.enlistedServices;

            return newEnlistState;
        }

        public override TransactionState Enlist(string serviceName)
        {
            if (enlistedServices.Contains(serviceName))
            {
                throw new TransactionException(State, TransactionStateEnum.Enlist, $"Service \"{serviceName}\" already enlisted.");
            }

            enlistedServices.Add(serviceName);

            return this;
        }

        public override TransactionState Prepare(string serviceName)
        {
            if (!isEnlistFinished)
            {
                throw new TransactionException(State, TransactionStateEnum.Prepare);
            }

            return new TransactionPrepareState(enlistedServices).Prepare(serviceName);
        }

        public override TransactionState Rollback(string serviceName)
        {
            if (enlistedServices.Count == 0)
            {
                return new TransactionIdleState();
            }

            return new TransactionRollbackState(enlistedServices).Rollback(serviceName);
        }

        public override TransactionState StartEnlist()
        {
            throw new TransactionException(State, TransactionStateEnum.Enlist, "Enlist already started!");
        }
    }
}
