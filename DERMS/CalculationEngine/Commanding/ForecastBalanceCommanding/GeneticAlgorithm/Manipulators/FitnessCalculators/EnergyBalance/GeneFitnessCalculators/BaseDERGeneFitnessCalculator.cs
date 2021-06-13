using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Model.EnergyBalance.Genes;

namespace CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Manipulators.FitnessCalculators.EnergyBalance.GeneFitnessCalculators
{
    public abstract class BaseDERGeneFitnessCalculator<T> : IDERGeneFitnessCalculator
        where T : DERGene
    {
        public ulong IntervalSimulation { get; set; }

        public double Calculate(DERGene derGene)
        {
            return Calculate(derGene as T);
        }

        protected abstract double Calculate(T gene);
    }
}
