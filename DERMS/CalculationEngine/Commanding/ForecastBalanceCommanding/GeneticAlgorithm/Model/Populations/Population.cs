using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Model.Chromosomes;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Model.Genes;
using System.Collections.Generic;

namespace CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Model.Populations
{
    public class Population<T>
        where T : Gene, new()
    {
        public Population()
        {
            Chromosomes = new List<Chromosome<T>>();

        }

        public List<Chromosome<T>> Chromosomes { get; set; }

        public Chromosome<T> GetBestChromosome()
        {
            int bestIndex = 0;
            double minFitnessValue = double.PositiveInfinity;

            for (int i = 0; i < Chromosomes.Count; i++)
            {
                Chromosome<T> chromosome = Chromosomes[i];
                if (minFitnessValue > chromosome.FitnessValue)
                {
                    bestIndex = i;
                    minFitnessValue = chromosome.FitnessValue;
                }
            }

            return Chromosomes[bestIndex];
        }
    }
}
