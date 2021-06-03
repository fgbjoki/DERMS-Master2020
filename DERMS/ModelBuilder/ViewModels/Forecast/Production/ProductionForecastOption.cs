using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientUI.ViewModels.Forecast.Production
{
    public enum ProductionForecastType
    {
        Total,
        Entity
    }

    public class ProductionForecastOption
    {
        private ProductionForecastType productionType;

        public ProductionForecastOption(string name, ProductionForecastType productionForecastType)
        {
            Name = name;
            ProductionForecastType = productionForecastType;
        }

        public string Name { get; set; }

        public ProductionForecastType ProductionForecastType { get; set; }
    }
}
