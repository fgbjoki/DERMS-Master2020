using Common.DataTransferObjects;
using System.ServiceModel;

namespace Common.ServiceInterfaces.UIAdapter
{
    [ServiceContract]
    public interface IBreakerCommanding
    {
        [OperationContract]
        CommandFeedbackMessageDTO SendBreakerCommand(long breakerGid, int breakerValue);

        [OperationContract]
        CommandFeedbackMessageDTO ValidateCommand(long breakerGid, int breakerValue);
    }
}
