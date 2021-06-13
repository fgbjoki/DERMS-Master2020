using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Helpers;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Model.EnergyBalance.Genes;
using Common.AbstractModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Manipulators.Mutators.EnergyBalance
{
    public class GridStateGeneMutator : EnergyBalanceGeneMutator<GridStateGene>
    {
        private EnergyStorageGeneMutator energyStorageMutator;
        private GeneratorGeneMutator generatorGeneMutator;

        private double mutationPercentage = 0.3f;

        public GridStateGeneMutator(EnergyStorageActivePowerCalculation esAPCalculator)
        {
            energyStorageMutator = new EnergyStorageGeneMutator(esAPCalculator);
            generatorGeneMutator = new GeneratorGeneMutator();
        }

        public override void Mutate(GridStateGene gene)
        {
            int numberOfGenesToMutate = Convert.ToInt32(Math.Round(gene.DERGenes.Count * mutationPercentage));
            foreach (var index in GetGeneMutationIndexes(gene.DERGenes, numberOfGenesToMutate))
            {
                DERGene derGene = gene.DERGenes[index];
                if (derGene.DMSType == DMSType.ENERGYSTORAGE)
                {
                    energyStorageMutator.Mutate(derGene as EnergyStorageGene);
                }
                else
                {
                    generatorGeneMutator.Mutate(derGene as GeneratorGene);
                }
            }
        }

        private int[] GetGeneMutationIndexes(List<DERGene> genes, int numberofGenesToMutate)
        {
            HashSet<int> disctinctIndexes = new HashSet<int>(numberofGenesToMutate);

            while (disctinctIndexes.Count < numberofGenesToMutate)
            {
                int index = random.Next(0, genes.Count);

                disctinctIndexes.Add(index);
            }

            return disctinctIndexes.ToArray();
        }
    }
}
