using Common.UIDataTransferObject.Forecast.Production;
using System.ServiceModel;

namespace Common.ServiceInterfaces.UIAdapter
{
    [ServiceContract]
    public interface IProductionForecast
    {
        [OperationContract]
        ProductionForecastDTO GetProductionForecast(int hours);
    }
}