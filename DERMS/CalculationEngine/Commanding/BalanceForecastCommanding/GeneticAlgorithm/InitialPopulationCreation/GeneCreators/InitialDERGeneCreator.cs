using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.InitialPopulationCreation.Model;
using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Model.Genes;

namespace CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.InitialPopulationCreation.GeneCreators
{
    public abstract class InitialDERGeneCreator<T, TT> : BaseInitialGeneCreator<T, TT>
        where T : DERGene
        where TT : DEREntity
    {
        protected override void PopulateFields(T gene, TT entity)
        {
            gene.GlobalId = entity.GlobalId;
            gene.ActivePower = entity.ActivePower;
            gene.NominalPower = entity.NominalPower;
        }
    }
}
