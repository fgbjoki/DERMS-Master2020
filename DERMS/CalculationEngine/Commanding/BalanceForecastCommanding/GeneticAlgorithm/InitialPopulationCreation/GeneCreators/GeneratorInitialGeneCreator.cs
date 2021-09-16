using CalculationEngine.Commanding.BalanceForecastCommanding.DataPreparation.Production;
using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Model.Genes;

namespace CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.InitialPopulationCreation.GeneCreators
{
    public class GeneratorInitialGeneCreator : InitialDERGeneCreator<GeneratorGene, GeneratorProduction>
    {
        protected override GeneratorGene CreateNewGene()
        {
            return new GeneratorGene();
        }

        protected override void PopulateFields(GeneratorGene gene, GeneratorProduction entity)
        {
            base.PopulateFields(gene, entity);
            gene.IsEnergized = random.Next(0, 2) == 1 ? true : false;
        }
    }
}
