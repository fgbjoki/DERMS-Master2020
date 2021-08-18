using Core.Common.GDA;
using Core.Common.ServiceInterfaces.NMS;
using Microsoft.ServiceFabric.Data;
using System;
using System.Fabric;

namespace NetworkModelService.ServiceProviders
{
    public class DeltaServiceProvider : INetworkModelDeltaContract
    {
        private StatefulServiceContext context;

        private Func<Delta, UpdateResult> applyDelta;

        public DeltaServiceProvider(StatefulServiceContext context, Func<Delta, UpdateResult> applyDelta)
        {
            this.context = context;
            
            this.applyDelta = applyDelta;
        }

        public UpdateResult ApplyUpdate(Delta delta)
        {
            ServiceEventSource.Current.ServiceMessage(context, "NMS - Apply Delta started");

            var result = applyDelta(delta);
            ServiceEventSource.Current.ServiceMessage(context, "NMS - Apply Delta finished");
            return result;

        }
    }
}
