using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Model;
using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Model.Genes;

namespace CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Fitness.Calculators.Chromosomes
{
    public abstract class BaseChromosomeFitnessCalculator<T>
        where T : Gene
    {
        public abstract void Calculate(Chromosome<T> chromosome);
    }
}
