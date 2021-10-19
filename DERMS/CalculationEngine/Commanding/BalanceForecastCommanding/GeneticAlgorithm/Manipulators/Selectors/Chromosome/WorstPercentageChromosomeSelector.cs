using System;
using System.Collections.Generic;
using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Model;

namespace CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Manipulators.Selectors.Chromosome
{
    public class WorstPercentageChromosomeSelector : BaseChromosomeSelector
    {
        private float worstPercentage;

        public WorstPercentageChromosomeSelector(float worstPercentage = 0.01f)
        {
            this.worstPercentage = worstPercentage;
        }

        public override List<Chromosome<T>> Select<T>(Population<T> population)
        {
            int numberOfSelectedChromosomes = Convert.ToInt32(Math.Round(population.Chromosomes.Count * worstPercentage));

            // OK?
            //return population.Chromosomes.GetRange(population.Chromosomes.Count - numberOfSelectedChromosomes - 1, population.Chromosomes.Count);
            return population.Chromosomes.GetRange(population.Chromosomes.Count - numberOfSelectedChromosomes - 1, numberOfSelectedChromosomes);
        }
    }
}
