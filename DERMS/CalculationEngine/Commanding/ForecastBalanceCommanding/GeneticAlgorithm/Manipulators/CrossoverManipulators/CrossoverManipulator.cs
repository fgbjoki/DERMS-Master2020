using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Manipulators.ChromosomeCreators;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Model.Chromosomes;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Model.Genes;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Model.Populations;
using System;

namespace CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Manipulators.CrossoverManipulators
{
    public class CrossoverManipulator
    {
        private static Random random = new Random();

        public void Crossover<T>(BaseChromosomeCreator<T> chromosomeCreator, Population<T> population, int populationSize) 
            where T : Gene, new()
        {
            int neededCrossOvers = populationSize - population.Chromosomes.Count;

            for (int i = 0; i < neededCrossOvers; i++)
            {
                int firstChromosomeIndex = random.Next(0, population.Chromosomes.Count);
                int secondChromosomeIndex;
                do
                {
                    secondChromosomeIndex = random.Next(0, population.Chromosomes.Count);
                } while (firstChromosomeIndex == secondChromosomeIndex);

                Chromosome<T> newChromosome = population.Chromosomes[firstChromosomeIndex].CreateNewChromosome();
                chromosomeCreator.Crossover(newChromosome, population.Chromosomes[firstChromosomeIndex], population.Chromosomes[secondChromosomeIndex]);
                population.Chromosomes.Add(newChromosome);
            }
        }

    }
}
