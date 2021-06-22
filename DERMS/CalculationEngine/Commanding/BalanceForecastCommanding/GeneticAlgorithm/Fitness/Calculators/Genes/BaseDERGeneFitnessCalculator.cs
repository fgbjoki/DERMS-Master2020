using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Model.Genes;

namespace CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Fitness.Calculators.Genes
{
    public abstract class BaseDERGeneFitnessCalculator<T> : IDERGeneFitnessCalculator
        where T : DERGene
    {
        public float Calculate(DERGene derGene, DomainParameters domainParameters)
        {
            return Calculate(derGene as T, domainParameters);
        }

        protected abstract float Calculate(T gene, DomainParameters domainParameters);
    }
}
