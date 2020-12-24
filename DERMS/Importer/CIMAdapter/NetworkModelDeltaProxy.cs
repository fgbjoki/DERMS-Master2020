using Common.ServiceInterfaces;
using System.ServiceModel;
using Common.GDA;

namespace FTN.ESI.SIMES.CIM.CIMAdapter
{
    class NetworkModelDeltaProxy : ClientBase<INetworkModelDeltaContract>, INetworkModelDeltaContract
    {
        public NetworkModelDeltaProxy(string endpointName)
			: base(endpointName)
		{
        }

        public UpdateResult ApplyUpdate(Delta delta)
        {
            return Channel.ApplyUpdate(delta);
        }
    }
}
