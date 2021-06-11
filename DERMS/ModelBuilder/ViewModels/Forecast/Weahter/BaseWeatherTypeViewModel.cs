using Common.DataTransferObjects;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ClientUI.ViewModels.Forecast.Weahter
{
    public abstract class BaseWeatherTypeViewModel : BaseViewModel
    {
        public BaseWeatherTypeViewModel(string type, string yAxisTitle, Color background)
        {
            YAxisTitle = yAxisTitle;
            Type = type;
            Background = new SolidColorBrush(background);
            Values = new ChartValues<float>();

            AxisXStep = 1;

            DateTime now = DateTime.Now;
            LabelFormater = value =>
            {
                var currTimeSpan = now + TimeSpan.FromHours(value);
                return currTimeSpan.ToString(@"HH\:00", System.Globalization.CultureInfo.InstalledUICulture);
            };
        }

        public abstract void PopulateValues(List<WeatherDataInfo> weatherData);

        public SolidColorBrush Background { get; set; }

        public ChartValues<float> Values { get; set; }

        public string Type { get; set; }

        public string YAxisTitle { get; set; }

        public double AxisXStep { get; set; }

        public Func<double, string> LabelFormater { get; set; }
    }
}
