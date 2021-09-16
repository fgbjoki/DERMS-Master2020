using System;
using System.Collections.Generic;
using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Model;

namespace CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Manipulators.Selectors.Chromosome
{
    public class TopPercentageChromosomeSelector : BaseChromosomeSelector
    {
        private float topPercentage;

        public TopPercentageChromosomeSelector(float topPercentage = 0.2f)
        {
            this.topPercentage = topPercentage;
        }

        public override List<Chromosome<T>> Select<T>(Population<T> population)
        {
            int numberOfSelectedChromosomes = Convert.ToInt32(Math.Round(population.Chromosomes.Count * topPercentage));

            return population.Chromosomes.GetRange(0, numberOfSelectedChromosomes);
        }
    }
}
