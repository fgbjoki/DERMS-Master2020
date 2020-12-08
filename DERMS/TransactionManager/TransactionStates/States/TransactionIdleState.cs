namespace TransactionManager.TransactionStates
{
    /// <inheritdoc/>
    sealed class TransactionIdleState : TransactionState
    {
        public TransactionIdleState() : base(TransactionStateEnum.Idle)
        {

        }

        public override TransactionState Commit(string serviceName)
        {
            throw new TransactionException(State, TransactionStateEnum.Commit);
        }

        public override TransactionState EndEnlist(bool successful)
        {
            throw new TransactionException(State, TransactionStateEnum.Idle, "Transaction is not in progress.");
        }

        public override TransactionState Enlist(string serviceName)
        {
            throw new TransactionException(State, TransactionStateEnum.Enlist, "Enlist is not started.");
        }

        public override TransactionState Prepare(string serviceName)
        {
            throw new TransactionException(State, TransactionStateEnum.Prepare);
        }

        public override TransactionState Rollback(string serviceName)
        {
            throw new TransactionException(State, TransactionStateEnum.Prepare);
        }

        public override TransactionState StartEnlist()
        {
            return new TransactionEnlistState(false);
        }
    }
}
