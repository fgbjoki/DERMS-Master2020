using Core.Common.AbstractModel;
using Core.Common.ServiceInterfaces.FEP.FEPStorage;
using Core.Common.Transaction.Models.FEP.FEPStorage;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Core.Common.Communication.ServiceFabric.FEP
{
    public class FEPStorageWCFClient : ClientBase<IFEPStorage>, IFEPStorage
    {
        public FEPStorageWCFClient() : base(new NetTcpBinding(), new EndpointAddress("net.tcp://localhost:12121/FEPStorage/IFEPStorage"))
        {
        }

        public async Task<List<RemotePoint>> GetEntities(List<DMSType> entityDMSType)
        {
            return await CreateChannel().GetEntities(entityDMSType);
        }

        public RemotePoint GetEntity(long globalId)
        {
            return CreateChannel().GetEntity(globalId);
        }

        public void UpdateAnalogRemotePointValue(long globalId, float value)
        {

        }

        public void UpdateDiscreteRemotePointValue(long globalId, float value)
        {
            
        }
    }
}
