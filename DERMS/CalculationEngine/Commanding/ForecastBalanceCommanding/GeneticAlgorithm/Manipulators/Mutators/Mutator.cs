using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Model.Genes;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Model.Populations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Manipulators.Mutators
{
    public class Mutator
    {
        private static Random random = new Random();
        private readonly float geneMutationRatio = 0.05f;

        public void Mutate<T>(BaseGeneMutator<T> geneMutator, Population<T> population, double mutationProbability) 
            where T : Gene, new()
        {
            int numberOfChromosomesToMutate = Convert.ToInt32(Math.Round(population.Chromosomes.Count * mutationProbability));
            foreach (var indexToMutate in GetMutationIndexes(population.Chromosomes.Count, numberOfChromosomesToMutate))
            {
                var chromosome = population.Chromosomes[indexToMutate];
                int numberOfGenesToMutate = Convert.ToInt32(Math.Round(chromosome.Genes.Count * geneMutationRatio));

                foreach (var geneIndexToMutate in GetMutationIndexes(chromosome.Genes.Count, numberOfGenesToMutate))
                {
                    var gene = chromosome.Genes[geneIndexToMutate];
                    geneMutator.Mutate(gene);
                }
            }
        }

        private int[] GetMutationIndexes(int capacitySize, int numberOfMutations)
        {
            HashSet<int> disctinctIndexes = new HashSet<int>(numberOfMutations);

            while (disctinctIndexes.Count < numberOfMutations)
            {
                int index = random.Next(0, capacitySize);

                disctinctIndexes.Add(index);
            }

            return disctinctIndexes.ToArray();
        }
    }
}
