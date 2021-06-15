using CalculationEngine.Extensions;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Model.EnergyBalance.Genes;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Helpers;

namespace CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Manipulators.Mutators.EnergyBalance
{
    public class EnergyStorageGeneMutator : EnergyBalanceGeneMutator<EnergyStorageGene>
    {
        private EnergyStorageActivePowerCalculation energyStoragePowerCalculator;

        public EnergyStorageGeneMutator(EnergyStorageActivePowerCalculation energyStoragePowerCalculator)
        {
            this.energyStoragePowerCalculator = energyStoragePowerCalculator;
        }

        public override void Mutate(EnergyStorageGene gene)
        {
            float minimalActivePower = energyStoragePowerCalculator.GetMinimumActivePower(gene.Capacity, gene.StateOfCharge, gene.NominalPower);
            float maximalActivePower = energyStoragePowerCalculator.GetMaximumActivePower(gene.Capacity, gene.StateOfCharge, gene.NominalPower);

            float randomActivePower = random.Next(minimalActivePower, maximalActivePower);
            gene.ActivePower = randomActivePower;
        }
    }
}
