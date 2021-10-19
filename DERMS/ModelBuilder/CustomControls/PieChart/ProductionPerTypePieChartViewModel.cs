using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientUI.CustomControls.PieChart
{
    public class ProductionPerTypePieChartViewModel : PieCharViewModel
    {
        public ProductionPerTypePieChartViewModel()
        {
            SeriesCollection = new SeriesCollection
            {
                new PieSeries()
                {
                    Title = "Solar production",
                    Values = new ChartValues<ObservableValue>() { new ObservableValue(0) },
                    LabelPoint = (param) => CreateLabel(param)
                },

                new PieSeries()
                {
                    Title = "Wind production",
                    Values = new ChartValues<ObservableValue>() { new ObservableValue(0) },
                    LabelPoint = (param) => CreateLabel(param)
                },

                new PieSeries()
                {
                    Title = "Energy storage production",
                    Values = new ChartValues<ObservableValue>() { new ObservableValue(0) },
                    LabelPoint = (param) => CreateLabel(param)
                }
            };
        }

        public float SolarEnergyProduced
        {
            set
            {
                try
                {
                    ((PieSeries)SeriesCollection[0]).Visibility = value >= 0 ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
                    ((ObservableValue)SeriesCollection[0].Values[0]).Value = value;
                }
                catch { }
            }
        }

        public float WindEnergyProduced
        {
            set
            {
                try
                {
                    ((PieSeries)SeriesCollection[1]).Visibility = value >= 0 ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
                    ((ObservableValue)SeriesCollection[1].Values[0]).Value = value;
                }
                catch { }
            }
        }

        public float EnergyStorageEnergyProduced
        {
            set
            {
                try
                {
                    ((PieSeries)SeriesCollection[2]).Visibility = value >= 0 ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
                    ((ObservableValue)SeriesCollection[2].Values[0]).Value = value;
                }
                catch { }
            }
        }

        public override string CreateLabel(ChartPoint point)
        {
            return string.Format("{0} kW", point.Y);
        }
    }
}
