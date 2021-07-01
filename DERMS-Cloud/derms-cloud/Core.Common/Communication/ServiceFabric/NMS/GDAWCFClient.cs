using Common.ServiceInterfaces;
using Core.Common.AbstractModel;
using Core.Common.GDA;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Common.Communication.ServiceFabric.NMS
{
    public class GDAWCFClient : BaseServiceFabricWCFClient<INetworkModelGDAContract>, INetworkModelGDAContract
    {
        public GDAWCFClient() : base("fabric:/ServiceFabricApp/NetworkModel/INetworkModelGDAContract")
        {

        }
        public int GetExtentValues(ModelCode entityType, List<ModelCode> propIds)
        {
            var client = BuildClient(1);
            return client.InvokeWithRetryAsync(x => Task.FromResult(x.Channel.GetExtentValues(entityType, propIds))).GetAwaiter().GetResult();
        }

        public int GetExtentValues(ModelCode entityType, List<ModelCode> propIds, List<long> gids)
        {
            var client = BuildClient(1);
            return client.InvokeWithRetryAsync(x => Task.FromResult(x.Channel.GetExtentValues(entityType, propIds, gids))).GetAwaiter().GetResult();
        }

        public int GetExtentValues(DMSType dmsType, List<ModelCode> propIds)
        {
            var client = BuildClient(1);
            return client.InvokeWithRetryAsync(x => Task.FromResult(x.Channel.GetExtentValues(dmsType, propIds))).GetAwaiter().GetResult();
        }

        public int GetExtentValues(DMSType dmsType, List<ModelCode> propIds, List<long> gids)
        {
            var client = BuildClient(1);
            return client.InvokeWithRetryAsync(x => Task.FromResult(x.Channel.GetExtentValues(dmsType, propIds, gids))).GetAwaiter().GetResult();
        }

        public ResourceDescription GetValues(long resourceId, List<ModelCode> propIds)
        {
            var client = BuildClient(1);
            return client.InvokeWithRetryAsync(x => Task.FromResult(x.Channel.GetValues(resourceId, propIds))).GetAwaiter().GetResult();
        }

        public bool IteratorClose(int id)
        {
            var client = BuildClient(1);
            return client.InvokeWithRetryAsync(x => Task.FromResult(x.Channel.IteratorClose(id))).GetAwaiter().GetResult();
        }

        public List<ResourceDescription> IteratorNext(int n, int id)
        {
            var client = BuildClient(1);
            return client.InvokeWithRetryAsync(x => Task.FromResult(x.Channel.IteratorNext(n, id))).GetAwaiter().GetResult();
        }

        public int IteratorResourcesLeft(int id)
        {
            var client = BuildClient(1);
            return client.InvokeWithRetryAsync(x => Task.FromResult(x.Channel.IteratorResourcesLeft(id))).GetAwaiter().GetResult();
        }
    }
}
