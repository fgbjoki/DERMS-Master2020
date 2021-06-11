using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Common.DataTransferObjects;

namespace ClientUI.ViewModels.Forecast.Weahter
{
    public class CloudCoverTypeViewModel : BaseWeatherTypeViewModel
    {
        public CloudCoverTypeViewModel() : base("Cloud cover", "Cloud cover [%]", Color.FromRgb(14, 170, 189))
        {
        }

        public override void PopulateValues(List<WeatherDataInfo> weatherData)
        {
            Values.Clear();
            foreach (var weatherSample in weatherData)
            {
                Values.Add(weatherSample.CloudCover);
            }
        }
    }
}
