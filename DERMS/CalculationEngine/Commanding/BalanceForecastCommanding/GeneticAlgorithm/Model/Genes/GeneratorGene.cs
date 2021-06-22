namespace CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Model.Genes
{
    public class GeneratorGene : DERGene
    {
        public bool IsEnergized { get; set; }

        public override void PopulateGene(Gene g)
        {
            base.PopulateGene(g);
            GeneratorGene generatorGene = g as GeneratorGene;
            if (generatorGene == null)
            {
                return;
            }

            generatorGene.IsEnergized = IsEnergized;
        }

        protected override Gene ReplicateGene()
        {
            return new GeneratorGene();
        }
    }
}
