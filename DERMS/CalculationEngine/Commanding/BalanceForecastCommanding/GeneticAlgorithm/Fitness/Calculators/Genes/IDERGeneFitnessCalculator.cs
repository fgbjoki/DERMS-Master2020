using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Model.Genes;

namespace CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Fitness.Calculators.Genes
{
    interface IDERGeneFitnessCalculator
    {
        float Calculate(DERGene derGene, DomainParameters domainParameters);
    }
}
