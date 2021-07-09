using Core.Common.ServiceInterfaces.FEP.FieldCommunicator;
using System.ServiceModel;

namespace Core.Common.Communication.ServiceFabric.FEP
{
    public class CommunicationServiceWCFClient : ClientBase<IFiledCommunicator>, IFiledCommunicator
    {
        public CommunicationServiceWCFClient() : base(new NetTcpBinding(), new EndpointAddress("net.tcp://localhost:12122/FieldCommunicationService/IFiledCommunicator"))
        {
        }
        public void Send(byte[] data)
        {
            CreateChannel().Send(data);
        }
    }
}
