using Core.Common.ServiceInterfaces.Transaction;
using Core.Common.Transaction;
using Microsoft.ServiceFabric.Data.Collections;
using NetworkManagementService;
using System;
using System.Fabric;

namespace NetworkModelService.ServiceProviders
{
    public class NMSTransactionParticipant : TransactionParticipant
    {
        public NMSTransactionParticipant(Microsoft.ServiceFabric.Data.IReliableStateManager stateManager, StatefulServiceContext context, Action<StatefulServiceContext, string, object[]> log) : base(stateManager, context, "NMS", log)
        {
        }

        protected override ITransaction GetTransactionObject()
        {
            var service = stateManager.GetOrAddAsync<IReliableDictionary<string, NetworkModel>>(TransactionParticipantString).GetAwaiter().GetResult();
            using (var tx = stateManager.CreateTransaction())
            {
                var transactionParticipant = service.TryGetValueAsync(tx, TransactionParticipantString).GetAwaiter().GetResult();
                if (!transactionParticipant.HasValue)
                {
                    log.Invoke(context, $"{serviceName} - Transaction instance not found! Cannot participate in transaction.", null);
                    return null;
                }

                return transactionParticipant.Value;
            }
        }
    }
}
