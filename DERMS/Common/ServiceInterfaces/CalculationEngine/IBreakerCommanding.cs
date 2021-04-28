using Common.Helpers.Breakers;
using System.ServiceModel;

namespace Common.ServiceInterfaces.CalculationEngine
{
    [ServiceContract]
    public interface IBreakerCommanding
    {
        void UpdateBreakers();

        [OperationContract]
        bool ValidateCommand(long breakerGid, BreakerState breakerState);

        [OperationContract]
        bool SendCommand(long breakerGid, int breakerValue);
    }
}
