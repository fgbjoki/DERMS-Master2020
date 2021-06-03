using System.Collections.Generic;
using Common.WeatherAPI;

namespace CalculationEngine.Forecast.WeatherForecast.DataConverter
{
    public interface IWeatherDataConverter
    {
        List<WeatherDataInfo> CovertData(List<WeatherDayData> apiData);
    }
}