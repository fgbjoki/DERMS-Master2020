using Core.Common.ServiceInterfaces.FEP.MessageAggregator;
using System.ServiceModel;

namespace Core.Common.Communication.ServiceFabric.FEP
{
    public class ResponseReceiverWCFClient : ClientBase<IResponseReceiver>, IResponseReceiver
    {
        public ResponseReceiverWCFClient() : base(new NetTcpBinding(), new EndpointAddress("net.tcp://localhost:12322/MessageAggregator/IResponseReceiver"))
        {
        }

        public void ReceiveCommand(byte[] receivedBytes)
        {
            CreateChannel().ReceiveCommand(receivedBytes);
        }
    }
}