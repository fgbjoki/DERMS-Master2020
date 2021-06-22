using System;

namespace CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Model.Genes
{
    public class EnergyStorageGene : DERGene
    {
        public float Capacity { get; set; }
        public float StateOfCharge { get; set; }

        public override void PopulateGene(Gene g)
        {
            base.PopulateGene(g);
            EnergyStorageGene energyStorageGene = g as EnergyStorageGene;
            if (energyStorageGene == null)
            {
                return;
            }

            energyStorageGene.Capacity = Capacity;
            energyStorageGene.StateOfCharge = StateOfCharge;
        }

        protected override Gene ReplicateGene()
        {
           return new EnergyStorageGene();
        }
    }
}
