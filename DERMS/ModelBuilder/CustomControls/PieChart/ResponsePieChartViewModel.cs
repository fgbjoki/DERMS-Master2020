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
    public class ResponsePieChartViewModel : PieCharViewModel
    {
        public ResponsePieChartViewModel()
        {
            SeriesCollection.Add(new PieSeries()
            {
                Title = "Imported",
                Values = new ChartValues<ObservableValue>() { new ObservableValue(0) },
                LabelPoint = (param) => CreateLabel(param)
            });

            SeriesCollection.Add(new PieSeries()
            {
                Title = "DERs",
                Values = new ChartValues<ObservableValue>() { new ObservableValue(0) },
                LabelPoint = (param) => CreateLabel(param)
            });
        }

        public float Imported
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
        public float DERs
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

        public override string CreateLabel(ChartPoint point)
        {
            return string.Format("{0} kW", point.Y);
        }
    }
}
