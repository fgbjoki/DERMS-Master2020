using System.Collections.Generic;

namespace Common.WeatherApiTester
{
    public interface IWeatherClient
    {
        WeatherDayData GetNextDayWeatherData();
    }
}
