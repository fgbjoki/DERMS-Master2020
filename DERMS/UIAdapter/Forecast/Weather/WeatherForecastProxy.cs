using Common.ServiceInterfaces.UIAdapter;
using System.Collections.Generic;
using Common.DataTransferObjects;
using Common.Communication;
using Common.ServiceInterfaces.CalculationEngine;

namespace UIAdapter.Forecast.Weather
{
    public class WeatherForecastProxy : IWeatherForecast
    {
        private WCFClient<IWeatherForecastStorage> weatherForecast;

        public WeatherForecastProxy()
        {
            weatherForecast = new WCFClient<IWeatherForecastStorage>("ceWeatherForecast");
        }

        public List<WeatherDataInfo> GetHourlyWeatherForecast(int hours)
        {
            try
            {
                return weatherForecast.Proxy.GetHourlyWeatherInfo(hours);
            }
            catch
            {
                return new List<WeatherDataInfo>();
            }
        }
    }
}
