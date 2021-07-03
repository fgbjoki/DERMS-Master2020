using System.ServiceModel;

namespace Core.Common.ServiceInterfaces.FEP.MessageAggregator
{
    [ServiceContract]
    public interface IResponseReceiver
    {
        [OperationContract]
        void ReceiveCommand(byte[] receivedBytes);
    }
}
