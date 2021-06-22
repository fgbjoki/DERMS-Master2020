using System;
using System.Collections.Generic;
using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Model;

namespace CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Manipulators.Selectors.Chromosome
{
    public class PercentageBasedSelection : BaseChromosomeSelector
    {
        private List<BaseChromosomeSelector> chromosomeSelectors;

        public PercentageBasedSelection(float topPercentage = 0.2f, float worstPercentage = 0.01f)
        {
            chromosomeSelectors = new List<BaseChromosomeSelector>()
            {
                new TopPercentageChromosomeSelector(topPercentage),
                new WorstPercentageChromosomeSelector(worstPercentage)
            };

        }

        public override List<Chromosome<T>> Select<T>(Population<T> population)
        {
            throw new NotImplementedException();
        }
    }
}
