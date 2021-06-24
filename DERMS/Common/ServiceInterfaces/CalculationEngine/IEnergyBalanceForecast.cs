using Common.DataTransferObjects.CalculationEngine.EnergyBalanceForecast;
using Common.UIDataTransferObject.EnergyBalanceForecast;
using System.ServiceModel;

namespace Common.ServiceInterfaces.CalculationEngine
{
    [ServiceContract]
    public interface IEnergyBalanceForecast
    {
        [OperationContract]
        DERStateCommandingSequenceDTO Compute(DomainParametersDTO domainParameters);
    }
}