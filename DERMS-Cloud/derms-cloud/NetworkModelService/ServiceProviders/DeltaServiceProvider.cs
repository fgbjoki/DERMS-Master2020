using Core.Common.GDA;
using Core.Common.ServiceInterfaces.NMS;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using NetworkManagementService;
using System;
using System.Fabric;

namespace NetworkModelService.ServiceProviders
{
    public class DeltaServiceProvider : INetworkModelDeltaContract
    {
        private readonly string nmsInstanceString;

        private IReliableStateManager stateManager;
        private StatefulServiceContext context;

        private Func<Delta, UpdateResult> applyDelta;

        public DeltaServiceProvider(IReliableStateManager stateManager, StatefulServiceContext context, Func<Delta, UpdateResult> applyDelta)
        {
            this.context = context;
            this.stateManager = stateManager;
            
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
