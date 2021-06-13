using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Model.Chromosomes;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Model.Genes;

namespace CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Manipulators.ChromosomeCreators
{
    public abstract class BaseChromosomeCreator<T>
        where T : Gene, new()
    {
        public abstract void Crossover(Chromosome<T> child, Chromosome<T> firstParent, Chromosome<T> secondParent);
    }
}
