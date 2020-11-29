using System.Collections.Generic;

namespace Common.WeatherApiTester
{
    public interface IWeatherClient
    {
        List<WeatherDayData> GetWeatherDayData(int numberOfDays);
    }
}
