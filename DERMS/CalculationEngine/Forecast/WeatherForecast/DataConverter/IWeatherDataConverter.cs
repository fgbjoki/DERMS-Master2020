using System.Collections.Generic;
using Common.WeatherAPI;
using Common.DataTransferObjects;

namespace CalculationEngine.Forecast.WeatherForecast.DataConverter
{
    public interface IWeatherDataConverter
    {
        List<WeatherDataInfo> CovertData(List<WeatherDayData> apiData);
    }
}