using Core.Common.GDA;
using Core.Common.ServiceInterfaces.NMS;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using NetworkManagementService;
using System.Fabric;

namespace NetworkModelService.ServiceProviders
{
    public class DeltaServiceProvider : INetworkModelDeltaContract
    {
        private readonly string nmsInstanceString;

        private IReliableStateManager stateManager;
        private StatefulServiceContext context;

        public DeltaServiceProvider(IReliableStateManager stateManager, StatefulServiceContext context, string nmsInstanceString)
        {
            this.context = context;
            this.stateManager = stateManager;
            
            this.nmsInstanceString = nmsInstanceString;
        }

        public UpdateResult ApplyUpdate(Delta delta)
        {
            ServiceEventSource.Current.ServiceMessage(context, "NMS - Apply Delta started");

            var reliableInstance = stateManager.GetOrAddAsync<IReliableDictionary<string, NetworkModel>>(nmsInstanceString).GetAwaiter().GetResult();

            using (var tx = stateManager.CreateTransaction())
            {
                var deltaContract = reliableInstance.TryGetValueAsync(tx, nmsInstanceString).GetAwaiter().GetResult();
                if (!deltaContract.HasValue)
                {
                    ServiceEventSource.Current.ServiceMessage(context, "NMS - Instance not found! Cannot apply delta.");
                    return new UpdateResult() { Result = ResultType.Failed, Message = "Internal error." };
                }

                var result = deltaContract.Value.ApplyUpdate(delta);
                ServiceEventSource.Current.ServiceMessage(context, "NMS - Apply Delta finished");
                return result;
            }
        }
    }
}
