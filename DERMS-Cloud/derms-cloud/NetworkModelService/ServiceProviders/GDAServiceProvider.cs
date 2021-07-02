using Common.ServiceInterfaces;
using Core.Common.AbstractModel;
using Core.Common.GDA;
using Core.Common.ListenerDepedencyInjection;
using NetworkManagementService;
using System.Collections.Generic;
using System.Fabric;

namespace NetworkModelService.ServiceProviders
{
    public class GDAServiceProvider : INetworkModelGDAContract
    {
        private StatefulServiceContext context;

        private ObjectProxy<NetworkModel> networkModelService;

        public GDAServiceProvider(StatefulServiceContext context, ObjectProxy<NetworkModel> networkModelService)
        {
            this.context = context;
            this.networkModelService = networkModelService;
        }

        public int GetExtentValues(ModelCode entityType, List<ModelCode> propIds)
        {
            ServiceEventSource.Current.ServiceMessage(context, $"NMS - GetExtentValues called for entity type: {entityType}");
            var result = networkModelService.Instance.GetExtentValues(entityType, propIds);
            ServiceEventSource.Current.ServiceMessage(context, $"NMS - Returned key for GetExtentValues: {result}");

            return result;
        }

        public int GetExtentValues(ModelCode entityType, List<ModelCode> propIds, List<long> gids)
        {
            ServiceEventSource.Current.ServiceMessage(context, $"NMS - GetExtentValues called for entity type: {entityType}");
            var result = networkModelService.Instance.GetExtentValues(entityType, propIds, gids);
            ServiceEventSource.Current.ServiceMessage(context, $"NMS - Returned key for GetExtentValues: {result}");

            return result;
        }

        public int GetExtentValues(DMSType dmsType, List<ModelCode> propIds)
        {
            ServiceEventSource.Current.ServiceMessage(context, $"NMS - GetExtentValues called for DMStype: {dmsType}");
            var result = networkModelService.Instance.GetExtentValues(dmsType, propIds);
            ServiceEventSource.Current.ServiceMessage(context, $"NMS - Returned key for GetExtentValues: {result}");

            return result;
        }

        public int GetExtentValues(DMSType dmsType, List<ModelCode> propIds, List<long> gids)
        {
            ServiceEventSource.Current.ServiceMessage(context, $"NMS - GetExtentValues called for DMStype: {dmsType}");
            var result = networkModelService.Instance.GetExtentValues(dmsType, propIds, gids);
            ServiceEventSource.Current.ServiceMessage(context, $"NMS - Returned key for GetExtentValues: {result}");

            return result;
        }

        public ResourceDescription GetValues(long resourceId, List<ModelCode> propIds)
        {
            ServiceEventSource.Current.ServiceMessage(context, $"NMS - GetExtentValues called for GlobalId: {resourceId}");
            var result = networkModelService.Instance.GetValues(resourceId, propIds);
            ServiceEventSource.Current.ServiceMessage(context, $"NMS - Returned ResourceDescription for GlobalId: {resourceId}");

            return result;
        }

        public bool IteratorClose(int id)
        {
            ServiceEventSource.Current.ServiceMessage(context, $"NMS - Iterator close called for id : {id}");
            var result = networkModelService.Instance.IteratorClose(id);
            ServiceEventSource.Current.ServiceMessage(context, $"NMS - Iterator close with id: {id} is {result}");

            return result;
        }

        public List<ResourceDescription> IteratorNext(int n, int id)
        {
            ServiceEventSource.Current.ServiceMessage(context, $"NMS - Iterator next called for id: {id} and capacity: {n}");
            var result = networkModelService.Instance.IteratorNext(n, id);
            ServiceEventSource.Current.ServiceMessage(context, $"NMS - Iterator next for id: {id} returning {result.Count} ResourceDescriptions.");

            return result;
        }

        public int IteratorResourcesLeft(int id)
        {
            ServiceEventSource.Current.ServiceMessage(context, $"NMS - Iterator resources left called for id: {id}");
            var result = networkModelService.Instance.IteratorResourcesLeft(id);
            ServiceEventSource.Current.ServiceMessage(context, $"NMS - Iterator resources left for id: {id}, has {result} resources left.");

            return result;
        }
    }
}
