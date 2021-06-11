using Common.Communication;
using Common.ServiceInterfaces.UIAdapter;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientUI.ViewModels.Forecast.Weahter
{
    public class WeatherForecastViewModel : BaseViewModel
    {
        private WCFClient<IWeatherForecast> weatherForcast;

        private List<BaseWeatherTypeViewModel> weatherTypeViewModels;
        public WeatherForecastViewModel()
        {
            weatherForcast = new WCFClient<IWeatherForecast>("uiWeatherForecast");
            TemperatureViewModel = new TemperatureTypeViewModel();
            WindViewModel = new WindTypeViewModel();
            CloudCoverViewModel = new CloudCoverTypeViewModel();

            weatherTypeViewModels = new List<BaseWeatherTypeViewModel>()
            {
                TemperatureViewModel,
                WindViewModel,
                CloudCoverViewModel,
            };
        }

        public void LoadWeatherForecastData()
        {
            try
            {
                var forecastData = weatherForcast.Proxy.GetHourlyWeatherForecast(24);
                foreach (var viewModel in weatherTypeViewModels)
                {
                    viewModel.PopulateValues(forecastData);
                }
            }
            catch
            {

            }
        }

        public TemperatureTypeViewModel TemperatureViewModel { get; set; }

        public WindTypeViewModel WindViewModel { get; set; }

        public CloudCoverTypeViewModel CloudCoverViewModel { get; set; }
    }
}
