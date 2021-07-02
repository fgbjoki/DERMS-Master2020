using Core.Common.ServiceInterfaces.NMS;
using Microsoft.ServiceFabric.Data;
using System;
using System.Collections.Generic;
using System.Fabric;

namespace Core.Common.Transaction
{
    public class TransactionParticipant<T> : ServiceInterfaces.Transaction.ITransaction, IModelPromotionParticipant
        where T : class, ServiceInterfaces.Transaction.ITransaction, IModelPromotionParticipant
    {
        protected IReliableStateManager stateManager;
        protected Action<StatefulServiceContext, string> log;
        protected StatefulServiceContext context;

        protected string serviceName;

        private ListenerDepedencyInjection.ObjectProxy<T> transactionObject;

        public TransactionParticipant(IReliableStateManager stateManager, StatefulServiceContext context, string serviceName, Action<StatefulServiceContext, string> log, ListenerDepedencyInjection.ObjectProxy<T> transactionObject)
        {
            this.stateManager = stateManager;
            this.context = context;

            this.serviceName = serviceName;
            this.transactionObject = transactionObject;
            this.log = log;
        }

        public bool ApplyChanges(List<long> insertedEntities, List<long> updatedEntities, List<long> deletedEntities)
        {
            log(context, $"{serviceName} - Apply changes called.");
            return transactionObject.Instance.ApplyChanges(insertedEntities, updatedEntities, deletedEntities);
        }

        public bool Commit()
        {
            log.Invoke(context, $"{serviceName} - Transaction commit called.");

            bool commited = transactionObject.Instance.Commit();

            log.Invoke(context, $"{serviceName} - Transaction commit {GetExecutionString(commited)}.");
            return commited;
        }

        public bool Prepare()
        {
            log.Invoke(context, $"{serviceName} - Transaction prepare called.");

            bool prepared = transactionObject.Instance.Prepare();

            log.Invoke(context, $"{serviceName} - Transaction prepare {GetExecutionString(prepared)}.");
            return prepared;
        }

        public bool Rollback()
        {
            log.Invoke(context, $"{serviceName} - Transaction rollback called.");

            bool rollbacked = transactionObject.Instance.Rollback();

            log.Invoke(context, $"{serviceName} - Transaction rollback {GetExecutionString(rollbacked)}.");
            return rollbacked;
        }

        private string GetExecutionString(bool executed)
        {
            return executed ? "succeeded" : "failed";
        }
    }
}
