using Common.ServiceInterfaces;
using Core.Common.AbstractModel;
using Core.Common.GDA;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Core.Common.Communication.ServiceFabric.NMS
{
    public class GDAWCFClient : ClientBase<INetworkModelGDAContract>, INetworkModelGDAContract
    {
        public GDAWCFClient() : base(new NetTcpBinding(),new EndpointAddress("net.tcp://localhost:11112/NetworkModel/INetworkModelGDAContract"))
        {

        }
        public int GetExtentValues(ModelCode entityType, List<ModelCode> propIds)
        {
            return CreateChannel().GetExtentValues(entityType, propIds);
        }

        public int GetExtentValues(ModelCode entityType, List<ModelCode> propIds, List<long> gids)
        {
            return CreateChannel().GetExtentValues(entityType, propIds, gids);
        }

        public int GetExtentValues(DMSType dmsType, List<ModelCode> propIds)
        {
            return CreateChannel().GetExtentValues(dmsType, propIds);
        }

        public int GetExtentValues(DMSType dmsType, List<ModelCode> propIds, List<long> gids)
        {
            
            return CreateChannel().GetExtentValues(dmsType, propIds, gids);
        }

        public ResourceDescription GetValues(long resourceId, List<ModelCode> propIds)
        {
            return CreateChannel().GetValues(resourceId, propIds);
        }

        public bool IteratorClose(int id)
        {
            return CreateChannel().IteratorClose(id);
        }

        public List<ResourceDescription> IteratorNext(int n, int id)
        {
            return CreateChannel().IteratorNext(n, id);
        }

        public int IteratorResourcesLeft(int id)
        {
            return CreateChannel().IteratorResourcesLeft(id);
        }
    }
}
