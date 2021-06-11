using Common.DataTransferObjects;
using System.Collections.Generic;
using System.ServiceModel;

namespace Common.ServiceInterfaces.UIAdapter
{
    [ServiceContract]
    public interface IWeatherForecast
    {
        [OperationContract]
        List<WeatherDataInfo> GetHourlyWeatherForecast(int hours);
    }
}
