using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Model.Genes;

namespace CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Manipulators.Mutators.Gene
{
    public interface IDERGeneMutator
    {
        void Mutate(DERGene derGene);
    }
}