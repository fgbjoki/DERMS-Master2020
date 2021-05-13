using Common.DataTransferObjects;
using System.ServiceModel;

namespace Common.ServiceInterfaces.UIAdapter
{
    [ServiceContract]
    public interface IDERCommanding
    {
        [OperationContract]
        CommandFeedbackMessageDTO SendCommand(long derGid, float commandingValue);

        [OperationContract]
        CommandFeedbackMessageDTO ValidateCommand(long derGid, float commandingValue);
    }
}
