using Common.DataTransferObjects;
using System.Collections.Generic;

namespace CalculationEngine.Commanding.ForecastBalanceCommanding.Production
{
    public class ProductionForecast
    {
        public ProductionForecast()
        {
            GeneratorProductions = new List<GeneratorProduction>();
        }

        public WeatherDataInfo WeahterData { get; set; }
        public List<GeneratorProduction> GeneratorProductions { get; set; }
    }
}
