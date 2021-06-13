using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Model.EnergyBalance.Genes;

namespace CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Manipulators.FitnessCalculators.EnergyBalance.GeneFitnessCalculators
{
    public class GeneratorGeneFitnessCalculator : BaseDERGeneFitnessCalculator<GeneratorGene>
    {
        public float CostOfShutdownPerKWH { get; set; }

        protected override double Calculate(GeneratorGene gene)
        {
            return gene.IsEnergized ? 0 : gene.ActivePower * IntervalSimulation / 3600 * CostOfShutdownPerKWH;
        }
    }
}
