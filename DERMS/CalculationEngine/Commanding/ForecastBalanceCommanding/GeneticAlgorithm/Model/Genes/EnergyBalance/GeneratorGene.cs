using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Model.Genes;

namespace CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Model.EnergyBalance.Genes
{
    public abstract class GeneratorGene : DERGene
    {
        public GeneratorGene(long globalId, float nominalPower, float activePower) : base(globalId, nominalPower, activePower)
        {
        }

        public bool IsEnergized { get; set; }

        public override void PopulateValues(Gene g)
        {
            base.PopulateValues(g);

            GeneratorGene generatorGene = g as GeneratorGene;
            if (generatorGene == null)
            {
                return;
            }

            generatorGene.IsEnergized = IsEnergized;
        }
    }
}
