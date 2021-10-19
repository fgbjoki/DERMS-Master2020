using System;
using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Model.Genes;

namespace CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Manipulators.Crossovers.Genes
{
    public class GeneratorGeneCrossover : BaseGeneCrossover<GeneratorGene>, IDERGeneCrossover
    {
        public DERGene Crossover(DERGene firstParent, DERGene secondParent)
        {
            return InternalCrossover(firstParent as GeneratorGene, secondParent as GeneratorGene);
        }

        public override GeneratorGene InternalCrossover(GeneratorGene firstParent, GeneratorGene secondParent)
        {
            GeneratorGene child = firstParent.Clone() as GeneratorGene;

            int parentGene = random.Next(0, 1);

            child.IsEnergized = parentGene == 1 ? firstParent.IsEnergized : secondParent.IsEnergized;

            return child;
        }
    }
}
