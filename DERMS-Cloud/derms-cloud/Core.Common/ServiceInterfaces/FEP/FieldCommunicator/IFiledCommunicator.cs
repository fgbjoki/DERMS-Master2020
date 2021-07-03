using System.ServiceModel;

namespace Core.Common.ServiceInterfaces.FEP.FieldCommunicator
{
    [ServiceContract]
    public interface IFiledCommunicator
    {
        [OperationContract]
        void Send(byte[] bytes);
    }
}
