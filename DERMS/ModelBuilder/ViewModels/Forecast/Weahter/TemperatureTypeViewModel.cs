using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Common.DataTransferObjects;

namespace ClientUI.ViewModels.Forecast.Weahter
{
    public class TemperatureTypeViewModel : BaseWeatherTypeViewModel
    {
        public TemperatureTypeViewModel() : base("Temperature", "Temperature [°C]", Color.FromRgb(187,101,101))
        {
        }

        public override void PopulateValues(List<WeatherDataInfo> weatherData)
        {
            Values.Clear();
            foreach (var weatherSample in weatherData)
            {
                Values.Add(weatherSample.TemperatureC);
            }
        }
    }
}
