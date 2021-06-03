using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientUI.ViewModels.Forecast.Production.ProductionSeriesManager
{
    public class ProductionSeriesManager
    {
        private Dictionary<PowerType, ChartValues<float>> chartValues;
        private DisplayTypeControl displayTypeControl;

        public ProductionSeriesManager(ChartValues<float> totalPower, ChartValues<float> solarPanelPower, ChartValues<float> windGeneratorPower, ChartValues<float> entityProduction, DisplayTypeControl displayTypeControl)
        {
            chartValues = new Dictionary<PowerType, ChartValues<float>>();
            chartValues.Add(PowerType.Total, totalPower);
            chartValues.Add(PowerType.Solar, solarPanelPower);
            chartValues.Add(PowerType.Wind, windGeneratorPower);
            chartValues.Add(PowerType.Entity, entityProduction);

            this.displayTypeControl = displayTypeControl;
        }

        public void PopulateChart(PowerType powerType, List<float> powerValues)
        {
            ChartValues<float> chartValue;
            if (!chartValues.TryGetValue(powerType, out chartValue))
            {
                return;
            }

            PopulateChart(chartValue, powerValues);
        }

        private void PopulateChart(ChartValues<float> chartValues, List<float> sampleValues)
        {
            chartValues.Clear();

            for (int i = 0; i < sampleValues.Count; ++i)
            {
                chartValues.Add(Convert.ToSingle(Math.Round((double)(sampleValues[i]), 2)));
            }
        }
    }
}
