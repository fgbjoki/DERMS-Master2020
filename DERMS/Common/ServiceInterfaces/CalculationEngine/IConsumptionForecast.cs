using Common.DataTransferObjects.CalculationEngine;
using System.ServiceModel;

namespace Common.ServiceInterfaces.CalculationEngine
{
    [ServiceContract]
    public interface IConsumptionForecast
    {
        [OperationContract]
        ForecastDTO ForecastConsumptionHourly(int hours);
    }
}
