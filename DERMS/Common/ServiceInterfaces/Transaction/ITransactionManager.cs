using System.ServiceModel;

namespace Common.ServiceInterfaces.Transaction
{
    [ServiceContract]
    public interface ITransactionManager
    {
        [OperationContract]
        bool StartEnlist();

        [OperationContract]
        void EnlistService(string serviceName);

        [OperationContract]
        bool EndEnlist(bool allServicesEnlisted);
    }
}
