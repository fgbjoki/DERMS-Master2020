using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Manipulators;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Model.Genes;

namespace CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Parameters
{
    public abstract class BaseGeneticAlgorithmParameters<T>
        where T: Gene, new()
    {
        public double ComputingTimeLimit { get; set; }

        public int PopulationSize { get; set; }

        public ManipulatorsWrapper<T> ManipulatorWrapper { get; set; }
    }
}
