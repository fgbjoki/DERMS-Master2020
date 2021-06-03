using System.Collections.Generic;

namespace CalculationEngine.Forecast.WeatherForecast
{
    public interface IWeatherForecastStorage
    {
        List<WeatherDataInfo> GetMinutesWeatherInfo(int minutes);

        List<WeatherDataInfo> GetHourlyWeatherInfo(int hours);
    }
}
