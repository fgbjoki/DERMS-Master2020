using Common.ServiceInterfaces;
using System.ServiceModel;
using Common.GDA;
using System.ServiceModel.Description;

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
