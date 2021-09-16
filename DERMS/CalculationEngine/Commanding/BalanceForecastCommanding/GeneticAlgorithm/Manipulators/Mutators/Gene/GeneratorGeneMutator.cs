using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Model.Genes;

namespace CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Manipulators.Mutators.Gene
{
    public class GeneratorGeneMutator : BaseGeneMutator<GeneratorGene>, IDERGeneMutator
    {
        public void Mutate(DERGene derGene)
        {
            Mutate(derGene as GeneratorGene);
        }

        public override void Mutate(GeneratorGene gene)
        {
            gene.IsEnergized = random.Next(0, 2) == 1 ? true : false;
        }
    }
}
