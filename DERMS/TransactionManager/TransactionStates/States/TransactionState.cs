namespace TransactionManager.TransactionStates
{
    public enum TransactionStateEnum
    {
        Idle,
        Enlist,
        Prepare,
        Commit,
        Rollback
    }

    /// <summary>
    /// Represents transaction state as a state machine.
    /// </summary>
    public abstract class TransactionState
    {
        protected TransactionState(TransactionStateEnum state)
        {
            State = state;
        }

        public TransactionStateEnum State { get; private set; }

        public abstract TransactionState StartEnlist();
        public abstract TransactionState Enlist(string serviceName);
        public abstract TransactionState EndEnlist(bool successful);

        public abstract TransactionState Prepare(string serviceName);
        public abstract TransactionState Commit(string serviceName);
        public abstract TransactionState Rollback(string serviceName);
    }
}
