using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.InitialPopulationCreators.Model;
using CalculationEngine.Commanding.ForecastBalanceCommanding.Production;
using System.Collections.Generic;

namespace CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.InitialPopulationCreators
{
    public class InputParameters
    {
        public List<float> EnergyDemand { get; set; }
        public List<ProductionForecast> Generators { get; set; }
        public List<EnergyStorageEntity> EnergyStorages { get; set; }
    }
}
