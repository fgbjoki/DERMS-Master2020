using Common.DataTransferObjects;
using Common.UIDataTransferObject.DEROptimalCommanding;
using System.ServiceModel;

namespace Common.ServiceInterfaces.UIAdapter
{
    [ServiceContract]
    public interface IDEROptimalCommandingProxy
    {
        [OperationContract]
        SuggsetedCommandSequenceDTO GetSuggestedCommandSequence(CommandRequestDTO commandSequenceRequest, float setPoint);

        [OperationContract]
        CommandFeedbackMessageDTO ExecuteCommandSequence(CommandSequenceRequest commandSequence);
    }
}