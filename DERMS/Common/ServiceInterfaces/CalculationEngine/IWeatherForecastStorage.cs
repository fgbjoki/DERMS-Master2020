using Common.DataTransferObjects;
using System.Collections.Generic;
using System.ServiceModel;

namespace Common.ServiceInterfaces.CalculationEngine
{
    [ServiceContract]
    public interface IWeatherForecastStorage
    {
        [OperationContract]
        List<WeatherDataInfo> GetMinutesWeatherInfo(int minutes);

        [OperationContract]
        List<WeatherDataInfo> GetHourlyWeatherInfo(int hours);

        List<WeatherDataInfo> GetNextDayWeatherInfo();
    }
}
