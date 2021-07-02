using Core.Common.GDA;
using Core.Common.ListenerDepedencyInjection;
using Core.Common.ServiceInterfaces.NMS;
using NetworkManagementService;
using System.Fabric;

namespace NetworkModelService.ServiceProviders
{
    public class DeltaServiceProvider : INetworkModelDeltaContract
    {
        private StatefulServiceContext context;

        private ObjectProxy<NetworkModel> networkModelService;

        public DeltaServiceProvider(StatefulServiceContext context, ObjectProxy<NetworkModel> networkModelService)
        {
            this.context = context;

            this.networkModelService = networkModelService;
        }

        public UpdateResult ApplyUpdate(Delta delta)
        {
            ServiceEventSource.Current.ServiceMessage(context, "NMS - Apply Delta started");

            var result = networkModelService.Instance.ApplyUpdate(delta);
            ServiceEventSource.Current.ServiceMessage(context, "NMS - Apply Delta finished");
            return result;
        }
    }
}
