namespace TransactionManager.TransactionStates
{
    /// <summary>
    /// Used to inject the reference of <seealso cref="TransactionState"/> into components for transaction logic.
    /// </summary>
    public class TransactionStateWrapper
    {
        public TransactionStateWrapper(TransactionState state)
        {
            CurrentState = state;
        }

        public TransactionState CurrentState { get; set; }
    }
}
