using Common.Helpers.Breakers;
using System.ServiceModel;

namespace Common.ServiceInterfaces.CalculationEngine
{
    [ServiceContract]
    public interface IBreakerLoopCommandingValidator
    {
        void UpdateBreakers();

        [OperationContract]
        bool ValidateCommand(long breakerGid, BreakerState breakerState);
    }
}
