using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Helpers;
using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Manipulators.Mutators.Chromosome;
using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Model;
using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Model.Genes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Manipulators.Mutators
{
    public class Mutator
    {
        private static Random random = new Random();
        private EnergyBalanceChromosomeMutator chromosomeMutator;

        public Mutator(EnergyStorageActivePowerCalculator energyStoragePowerCalculator)
        {
            chromosomeMutator = new EnergyBalanceChromosomeMutator(energyStoragePowerCalculator);
        }

        public void Mutate<T>(Population<T> population, GeneticAlgorithmParameters gaParameters) 
            where T : Model.Genes.Gene
        {
            int numberOfChromosomesToMutate = Convert.ToInt32(Math.Round(population.Chromosomes.Count * gaParameters.MutationRate));

            foreach (var chromosomeIndex in GetMutationIndexes(population.Chromosomes.Count, numberOfChromosomesToMutate))
            {
                chromosomeMutator.Mutate(population.Chromosomes[chromosomeIndex] as Chromosome<DERGene>);
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
