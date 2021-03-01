using System;
using TransactionManager.TransactionStates;

namespace TransactionManager
{
    /// <summary>
    /// Exception used for invalid transation state transitions.
    /// </summary>
    public class TransactionException : Exception
    {
        public TransactionException(TransactionStateEnum fromState, TransactionStateEnum toState, string message = "") : base($"Invalid transaction state transmition: From {fromState.ToString()} to {toState.ToString()}. {AdditionalExceptionMessage(message)}")
        {
        }

        private static string AdditionalExceptionMessage(string message)
        {
            return message.Trim() == String.Empty ? String.Empty : $"Additional info: {message}";
        }
    }
}
