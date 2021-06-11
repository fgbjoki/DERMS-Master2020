using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Common.DataTransferObjects;

namespace ClientUI.ViewModels.Forecast.Weahter
{
    public class WindTypeViewModel : BaseWeatherTypeViewModel
    {
        public WindTypeViewModel() : base("Wind", "Wind speed [m/s]", Color.FromRgb(40, 112, 184))
        {
        }

        public override void PopulateValues(List<WeatherDataInfo> weatherData)
        {
            Values.Clear();
            foreach (var weatherSample in weatherData)
            {
                Values.Add(weatherSample.WindMPS);
            }
        }
    }
}
