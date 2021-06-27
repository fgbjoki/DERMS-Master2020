using Core.Common.GDA;
using Core.Common.ServiceInterfaces.NMS;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using System.Fabric;
using System.Threading.Tasks;

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

        public async Task<UpdateResult> ApplyUpdate(Delta delta)
        {
            ServiceEventSource.Current.ServiceMessage(context, "NMS - Apply Delta started");

            var reliableInstance = await stateManager.GetOrAddAsync<IReliableDictionary<string, INetworkModelDeltaContract>>(nmsInstanceString);

            using (var tx = stateManager.CreateTransaction())
            {
                var deltaContract = await reliableInstance.TryGetValueAsync(tx, nmsInstanceString);
                if (!deltaContract.HasValue)
                {
                    ServiceEventSource.Current.ServiceMessage(context, "NMS - Instance not found! Cannot apply delta.");
                    return new UpdateResult() { Result = ResultType.Failed, Message = "Internal error." };
                }

                var result = await deltaContract.Value.ApplyUpdate(delta);
                ServiceEventSource.Current.ServiceMessage(context, "NMS - Apply Delta finished");
                return result;
            }
        }
    }
}
