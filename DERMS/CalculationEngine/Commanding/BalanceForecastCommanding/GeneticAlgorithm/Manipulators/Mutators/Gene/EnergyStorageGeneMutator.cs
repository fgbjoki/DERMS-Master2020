using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Helpers;
using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Model.Genes;

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
            InternalMutate(derGene as EnergyStorageGene);
        }

        public override void InternalMutate(EnergyStorageGene gene)
        {
            float minimalActivePower = energyStoragePowerCalculator.GetMinimumActivePower(gene.Capacity, gene.StateOfCharge, gene.NominalPower);
            float maximalActivePower = energyStoragePowerCalculator.GetMaximumActivePower(gene.Capacity, gene.StateOfCharge, gene.NominalPower);

            double range = maximalActivePower - minimalActivePower;
            double sample = random.NextDouble();
            double scaled = (sample * range) + minimalActivePower;
            
            float randomActivePower = (float)scaled;
            gene.ActivePower = randomActivePower;
        }
    }
}
