using Microsoft.ServiceFabric.Data;
using System;
using System.Fabric;

namespace Core.Common.Transaction
{
    public class TransactionParticipant : ServiceInterfaces.Transaction.ITransaction
    {
        protected IReliableStateManager stateManager;
        protected Action<StatefulServiceContext, string> log;
        protected StatefulServiceContext context;

        protected string serviceName;

        private TransactionSteps transactionSteps;

        public static string TransactionParticipantString { get; } = "TransactionParticipants";

        public TransactionParticipant(IReliableStateManager stateManager, StatefulServiceContext context, string serviceName, Action<StatefulServiceContext, string> log, TransactionSteps transactionSteps)
        {
            this.stateManager = stateManager;
            this.context = context;

            this.serviceName = serviceName;
            this.transactionSteps = transactionSteps;
            this.log = log;
        }

        public bool Commit()
        {
            log.Invoke(context, $"{serviceName} - Transaction commit called.");

            bool commited = transactionSteps.Commit();

            log.Invoke(context, $"{serviceName} - Transaction commit {GetExecutionString(commited)}.");
            return commited;
        }

        public bool Prepare()
        {
            log.Invoke(context, $"{serviceName} - Transaction prepare called.");

            bool prepared = transactionSteps.Prepare();

            log.Invoke(context, $"{serviceName} - Transaction prepare {GetExecutionString(prepared)}.");
            return prepared;
        }

        public bool Rollback()
        {
            log.Invoke(context, $"{serviceName} - Transaction rollback called.");

            bool rollbacked = transactionSteps.Rollback();

            log.Invoke(context, $"{serviceName} - Transaction rollback {GetExecutionString(rollbacked)}.");
            return rollbacked;
        }

        private string GetExecutionString(bool executed)
        {
            return executed ? "succeeded" : "failed";
        }
    }
}
