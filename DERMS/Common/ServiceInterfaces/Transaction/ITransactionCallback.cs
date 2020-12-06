using System.ServiceModel;

namespace Common.ServiceInterfaces.Transaction
{
    [ServiceContract]
    public interface ITransactionCallback
    {
        [OperationContract]
        bool Prepare();

        [OperationContract]
        bool Commit();

        [OperationContract]
        bool Rollback();
    }
}
