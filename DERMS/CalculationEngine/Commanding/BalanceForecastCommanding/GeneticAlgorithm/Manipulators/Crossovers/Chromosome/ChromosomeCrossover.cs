using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Helpers;
using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Manipulators.Crossovers.Genes;
using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Model;
using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Model.Genes;
using Common.AbstractModel;
using Common.Logger;
using System.Collections.Generic;

namespace CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Manipulators.Crossovers.Chromosome
{
    public class ChromosomeCrossover
    {
        private Dictionary<DMSType, IDERGeneCrossover> crossovers;

        public ChromosomeCrossover(EnergyStorageActivePowerCalculator esAPCalculator)
        {
            IDERGeneCrossover generatorCrossver = new GeneratorGeneCrossover();

            crossovers = new Dictionary<DMSType, IDERGeneCrossover>()
            {
                { DMSType.ENERGYSTORAGE, new EnergyStorageGeneCrossover(esAPCalculator) },
                { DMSType.SOLARGENERATOR, generatorCrossver },
                { DMSType.WINDGENERATOR, generatorCrossver }
            };
        }

        public Chromosome<T> Crossover<T>(Chromosome<T> firstParent, Chromosome<T> secondParent) 
            where T : DERGene
        {
            Chromosome<T> newChromosome = new Chromosome<T>();

            for (int i = 0; i < firstParent.Genes.Count; i++)
            {
                var firstParentGene = firstParent.Genes[i];
                var secondParentGene = secondParent.Genes[i];

                IDERGeneCrossover crossover;
                if (!crossovers.TryGetValue(firstParentGene.DMSType, out crossover))
                {
                    Logger.Instance.Log($"[{GetType().Name}] Cannot find crossover for DMSType: {firstParentGene.DMSType}");
                    continue;
                }

                var newGene = crossover.Crossover(firstParentGene, secondParentGene);
                newChromosome.Genes.Add(newGene as T);
            }

            return newChromosome;
        }
    }
}
