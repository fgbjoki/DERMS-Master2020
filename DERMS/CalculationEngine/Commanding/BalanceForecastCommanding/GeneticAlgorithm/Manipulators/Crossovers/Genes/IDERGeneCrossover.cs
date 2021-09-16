using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Model.Genes;

namespace CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Manipulators.Crossovers.Genes
{
    public interface IDERGeneCrossover
    {
        DERGene Crossover(DERGene firstParent, DERGene secondParent);
    }
}