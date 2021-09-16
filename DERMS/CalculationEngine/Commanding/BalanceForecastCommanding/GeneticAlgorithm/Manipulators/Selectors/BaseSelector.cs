using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Manipulators.Selectors.Chromosome;
using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Model;
using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Model.Genes;
using System.Collections.Generic;

namespace CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Manipulators.Selectors
{
    public class Selector
    {
        public Population<T> Select<T>(Population<T> currentPopulation, List<BaseChromosomeSelector> selectors ) where T : Gene
        {
            Population<T> newPopulation = new Population<T>();

            foreach (var selector in selectors)
            {
                var chromosomes = selector.Select(currentPopulation);
                newPopulation.Chromosomes.AddRange(chromosomes);
            }

            return newPopulation;
        }
    }
}
