using System.ServiceModel;

namespace Common.ServiceInterfaces.Transaction
{
    [ServiceContract(CallbackContract = typeof(ITransactionCallback))]
    public interface ITransactionManager
    {
        [OperationContract]
        bool StartEnlist();

        [OperationContract]
        bool EnlistService(string serviceName);

        [OperationContract]
        bool EndEnlist(bool allServicesEnlisted);
    }
}
