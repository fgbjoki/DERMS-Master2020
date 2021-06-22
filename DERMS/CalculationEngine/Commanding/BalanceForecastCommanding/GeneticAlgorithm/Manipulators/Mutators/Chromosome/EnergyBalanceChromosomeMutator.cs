using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Helpers;
using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Manipulators.Mutators.Gene;
using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Model.Genes;
using Common.AbstractModel;
using System;
using System.Collections.Generic;
using System.Linq;
using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Model;
using Common.Logger;

namespace CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Manipulators.Mutators.Chromosome
{
    public class EnergyBalanceChromosomeMutator : BaseChromosomeMutator<DERGene>
    {
        private Dictionary<DMSType, IDERGeneMutator> geneMutators;

        public EnergyBalanceChromosomeMutator(EnergyStorageActivePowerCalculator energyStoragePowerCalculator)
        {
            IDERGeneMutator generatorMutator = new GeneratorGeneMutator();

            geneMutators = new Dictionary<DMSType, IDERGeneMutator>()
            {
                { DMSType.ENERGYSTORAGE, new EnergyStorageGeneMutator(energyStoragePowerCalculator) },
                { DMSType.SOLARGENERATOR, generatorMutator },
                { DMSType.WINDGENERATOR, generatorMutator }
            };
        }

        public override void Mutate(Chromosome<DERGene> chromosome)
        {
            int numberOfGeneMutation = Convert.ToInt32(Math.Round(geneMutationRate * chromosome.Genes.Count));

            foreach (var geneIndex in GetMutationIndexes(chromosome.Genes.Count, numberOfGeneMutation))
            {
                DERGene gene = chromosome.Genes[geneIndex];

                IDERGeneMutator geneMutator;
                if (!geneMutators.TryGetValue(gene.DMSType, out geneMutator))
                {
                    Logger.Instance.Log($"[{GetType().Name}] Couldn't find mutator for DMSType: {gene.DMSType}");
                    continue;
                }

                geneMutator.Mutate(gene);
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
