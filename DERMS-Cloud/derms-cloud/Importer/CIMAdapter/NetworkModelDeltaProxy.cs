using System.ServiceModel;
using System.Threading.Tasks;
using Core.Common.GDA;
using Core.Common.ServiceInterfaces.NMS;

namespace FTN.ESI.SIMES.CIM.CIMAdapter
{
    class NetworkModelDeltaProxy : ClientBase<INetworkModelDeltaContract>, INetworkModelDeltaContract
    {
        public NetworkModelDeltaProxy(string endpointAddress)
			: base(new NetTcpBinding(), new EndpointAddress(endpointAddress))
		{
        }

        public UpdateResult ApplyUpdate(Delta delta)
        {
            return Channel.ApplyUpdate(delta);
        }
    }
}
