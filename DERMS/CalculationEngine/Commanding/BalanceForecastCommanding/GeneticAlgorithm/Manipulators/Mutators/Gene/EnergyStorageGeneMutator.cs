using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Helpers;
using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Model.Genes;
using CalculationEngine.Extensions;

namespace CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Manipulators.Mutators.Gene
{
    public class EnergyStorageGeneMutator : BaseGeneMutator<EnergyStorageGene>, IDERGeneMutator
    {
        private EnergyStorageActivePowerCalculator energyStoragePowerCalculator;

        public EnergyStorageGeneMutator(EnergyStorageActivePowerCalculator energyStoragePowerCalculator)
        {
            this.energyStoragePowerCalculator = energyStoragePowerCalculator;
        }

        public void Mutate(DERGene derGene)
        {
            Mutate(derGene as EnergyStorageGene);
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
