using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.InitialPopulationCreators.Model;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Model.EnergyBalance.Genes;

namespace CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.InitialPopulationCreators.GeneCreators
{
    public abstract class InitialDERGeneCreator<T, TT> : BaseInitialGeneCreator<T, TT>
        where T : DERGene
        where TT : DEREntity
    {
        protected virtual void PopulateFields(T gene, TT entity)
        {
            gene.GlobalId = entity.GlobalId;
            gene.ActivePower = entity.ActivePower;
            gene.NominalPower = entity.NominalPower;
        }
    }
}
