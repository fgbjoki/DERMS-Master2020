using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Model.Genes;

namespace CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Model.EnergyBalance.Genes
{
    public class EnergyStorageGene : DERGene
    {
        public EnergyStorageGene(long globalId, float nominalPower, float activePower) : base(globalId, nominalPower, activePower)
        {
        }

        public float StateOfCharge { get; set; }

        public float Capacity { get; set; }

        public override void PopulateValues(Gene g)
        {
            base.PopulateValues(g);

            EnergyStorageGene energyStorageGene = g as EnergyStorageGene;
            if (energyStorageGene == null)
            {
                return;
            }

            energyStorageGene.StateOfCharge = StateOfCharge;
            energyStorageGene.Capacity = Capacity;
        }
    }
}
