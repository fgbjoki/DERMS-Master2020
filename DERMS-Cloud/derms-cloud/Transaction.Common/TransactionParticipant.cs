using Microsoft.ServiceFabric.Data;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transaction.Common
{
    public class TransactionParticipant : Core.Common.ServiceInterfaces.Transaction.ITransaction
    {
        protected IReliableStateManager stateManager;
        protected Action<StatefulServiceContext, string, object[]> log;
        protected StatefulServiceContext context;

        protected string serviceName;


        public static string TransactionParticipantString { get; } = "TransactionParticipants";

        public TransactionParticipant(IReliableStateManager stateManager, StatefulServiceContext context, string serviceName, Action<StatefulServiceContext, string, object[]> log)
        {
            this.stateManager = stateManager;
            this.context = context;

            this.serviceName = serviceName;

            this.log = log;
        }

        public bool Commit()
        {
            //log.Invoke(context, $"{serviceName} - Transaction commit called.", null);
            //ServiceInterfaces.Transaction.ITransaction transactionObject = GetTransactionObject();

            //bool commited = transactionObject.Commit();

            //log.Invoke(context, $"{serviceName} - Transaction commit {GetExecutionString(commited)}.", null);
            //return commited;

            return true;
        }

        //protected abstract ServiceInterfaces.Transaction.ITransaction GetTransactionObject();

        public bool Prepare()
        {
            //log.Invoke(context, $"{serviceName} - Transaction prepare called.", null);
            //ServiceInterfaces.Transaction.ITransaction transactionObject = GetTransactionObject();

            //bool commited = transactionObject.Prepare();

            //log.Invoke(context, $"{serviceName} - Transaction prepare {GetExecutionString(commited)}.", null);
            //return commited;

            return true;
        }

        public bool Rollback()
        {
            //log.Invoke(context, $"{serviceName} - Transaction rollback called.", null);
            //ServiceInterfaces.Transaction.ITransaction transactionObject = GetTransactionObject();

            //bool commited = transactionObject.Rollback();

            //log.Invoke(context, $"{serviceName} - Transaction rollback {GetExecutionString(commited)}.", null);
            //return commited;

            return true;
        }

        private string GetExecutionString(bool executed)
        {
            return executed ? "succeeded" : "failed";
        }
    }
}
