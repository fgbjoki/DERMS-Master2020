using Common.DataTransferObjects.CalculationEngine.DEROptimalCommanding;
using Common.ServiceInterfaces.CalculationEngine.DEROptimalCommanding;
using System.ServiceModel;

namespace Common.ServiceInterfaces.CalculationEngine
{
    [ServiceContract]
    public interface IDEROptimalCommanding
    {
        [OperationContract]
        DEROptimalCommandingFeedbackDTO CreateCommand(DEROptimalCommand command);
    }
}
