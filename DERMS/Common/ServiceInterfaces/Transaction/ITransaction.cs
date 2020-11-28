using System.ServiceModel;

namespace Common.ServiceInterfaces.Transaction
{
    [ServiceContract]
    public interface ITransaction
    {
        [OperationContract]
        bool Prepare();

        [OperationContract]
        bool Commit();

        [OperationContract]
        bool Rollback();
    }
}
