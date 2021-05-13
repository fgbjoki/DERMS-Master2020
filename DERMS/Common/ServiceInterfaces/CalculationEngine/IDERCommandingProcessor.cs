using Common.DataTransferObjects.CalculationEngine;
using System.ServiceModel;

namespace Common.ServiceInterfaces.CalculationEngine
{
    [ServiceContract]
    public interface IDERCommandingProcessor
    {
        [OperationContract]
        CommandFeedback Command(long derGid, float commandingValue);

        [OperationContract]
        CommandFeedback ValidateCommand(long derGid, float commandingValue);
    }
}