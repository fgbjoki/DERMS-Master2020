using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Model.EnergyBalance.Genes;

namespace CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Manipulators.FitnessCalculators.EnergyBalance.GeneFitnessCalculators
{
    public class EnergyStorageGeneFitnessCalculator : BaseDERGeneFitnessCalculator<EnergyStorageGene>
    {
        public double CostOfEnergyUse { get; set; }

        protected override double Calculate(EnergyStorageGene gene)
        {
            return gene.ActivePower * CostOfEnergyUse * IntervalSimulation / 3600;
        }
    }
}
