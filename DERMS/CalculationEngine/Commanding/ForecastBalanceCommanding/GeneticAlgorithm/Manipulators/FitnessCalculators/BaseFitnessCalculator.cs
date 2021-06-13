using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Model.Chromosomes;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Model.Genes;

namespace CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Manipulators.FitnessCalculators
{
    public abstract class BaseFitnessCalculator<GeneType, FitnessParameterType>
        where GeneType : Gene, new()
    {
        public abstract double Calculate(Chromosome<GeneType> chromosome);
    }
}
