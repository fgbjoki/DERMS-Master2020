using CalculationEngine.Commanding.BalanceForecastCommanding.DataPreparation.Production;
using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.InitialPopulationCreation.Model;
using System.Collections.Generic;

namespace CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.InitialPopulationCreation
{
    public class InitialPopulationCreationParameters
    {
        public List<GeneratorProduction> Generators { get; set; }
        public List<EnergyStorageEntity> EnergyStorages { get; set; }
        public List<EnergyConsumerEntity> EnergyConsumers { get; set; }
    }
}
