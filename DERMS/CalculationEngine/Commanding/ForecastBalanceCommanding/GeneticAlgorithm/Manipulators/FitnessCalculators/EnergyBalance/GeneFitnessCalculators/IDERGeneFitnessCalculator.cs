using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Model.EnergyBalance.Genes;

namespace CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Manipulators.FitnessCalculators.EnergyBalance.GeneFitnessCalculators
{
    public interface IDERGeneFitnessCalculator
    {
        double Calculate(DERGene derGene);

        ulong IntervalSimulation { get; set; }
    }
}