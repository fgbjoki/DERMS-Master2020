using System.ServiceModel;

namespace Common.ServiceInterfaces.UIAdapter
{
    [ServiceContract]
    public interface IBreakerCommanding
    {
        [OperationContract]
        bool SendBreakerCommand(long breakerGid, int breakerValue);

        [OperationContract]
        bool ValidateCommand(long breakerGid, int breakerValue);
    }
}
