using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Model.Genes;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Model.Populations;

namespace CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Manipulators.Selectors
{
    public abstract class BaseChromosomeSelector<T>
        where T : Gene, new()
    {
        public abstract void Select(Population<T> sourcePopulation, Population<T> destinationPopulation);
    }
}
