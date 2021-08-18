using Common.DataTransferObjects.CalculationEngine;
using System.ServiceModel;

namespace Common.ServiceInterfaces.CalculationEngine
{
    [ServiceContract]
    public interface IProductionForecast
    {
        [OperationContract]
        ForecastDTO ForecastProductionMinutely(int minutes);

        [OperationContract]
        ForecastDTO ForecastProductionHourly(int hours);
    }
}