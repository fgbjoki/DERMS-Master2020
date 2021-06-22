using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Model;
using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Model.Genes;
using System.Collections.Generic;

namespace CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Manipulators.Selectors.Chromosome
{
    public abstract class BaseChromosomeSelector
    {
        public abstract List<Chromosome<T>> Select<T>(Population<T> population) where T : Gene;
    }
}
