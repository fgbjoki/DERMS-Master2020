using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Helpers;
using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Manipulators.Crossovers.Chromosome;
using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Model;
using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Model.Genes;
using System;
using System.Collections.Generic;

namespace CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Manipulators.Crossovers
{
    public class PopulationCrossover
    {
        private static Random random = new Random();

        private ChromosomeCrossover chromosomeCrossover;

        public PopulationCrossover(EnergyStorageActivePowerCalculator esAPCalculator)
        {
            chromosomeCrossover = new ChromosomeCrossover(esAPCalculator);
        }

        public void Crossover<T>(Population<T> population, int finalPopulationSize) where T : DERGene
        {
            List<Chromosome<T>> newChromosomes = new List<Chromosome<T>>(finalPopulationSize - population.Chromosomes.Count);

            int currentPopulationSize = population.Chromosomes.Count;
            for (int i = 0; i < finalPopulationSize - currentPopulationSize; i++)
            {
                int[] uniqueIndexes = GetUniqueIndexes(currentPopulationSize);

                var newChromosome = chromosomeCrossover.Crossover(population.Chromosomes[uniqueIndexes[0]], population.Chromosomes[uniqueIndexes[1]]);

                population.Chromosomes.Add(newChromosome);
            }
        }

        private int[] GetUniqueIndexes(int populationSize)
        {
            int firstIndex = random.Next(0, populationSize);
            int secondIndex;

            do
            {
                secondIndex = random.Next(0, populationSize);
            } while (secondIndex == firstIndex);


            return new int[2] { firstIndex, secondIndex };
        }
    }
}
