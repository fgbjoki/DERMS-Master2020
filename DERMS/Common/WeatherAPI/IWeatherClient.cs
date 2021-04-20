using System.Collections.Generic;

namespace Common.WeatherAPI
{
    public interface IWeatherClient
    {
        WeatherDayData GetCurrentDayWeatherData();
        List<WeatherDayData> GetWeatherData(int days);
    }
}
