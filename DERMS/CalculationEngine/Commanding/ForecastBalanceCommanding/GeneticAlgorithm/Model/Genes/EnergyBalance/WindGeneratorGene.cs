using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Model.Genes;

namespace CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Model.EnergyBalance.Genes
{
    public class WindGeneratorGene : GeneratorGene
    {
        public WindGeneratorGene(long globalId, float nominalPower, float activePower) : base(globalId, nominalPower, activePower)
        {
        }

        public float CutOutSpeed { get; set; }
        public float StartUpSpeed { get; set; }
        public float NominalSpeed { get; set; }

        public override void PopulateValues(Gene g)
        {
            base.PopulateValues(g);

            WindGeneratorGene windGeneratorGene = g as WindGeneratorGene;
            if (windGeneratorGene == null)
            {
                return;
            }

            windGeneratorGene.CutOutSpeed = CutOutSpeed;
            windGeneratorGene.StartUpSpeed = StartUpSpeed;
            windGeneratorGene.NominalSpeed = NominalSpeed;
        }
    }
}
