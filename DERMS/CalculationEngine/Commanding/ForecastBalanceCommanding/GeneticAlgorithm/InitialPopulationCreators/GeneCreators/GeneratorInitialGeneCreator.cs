using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.InitialPopulationCreators.Model;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Model.EnergyBalance.Genes;
using CalculationEngine.Commanding.ForecastBalanceCommanding.Production;

namespace CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.InitialPopulationCreators.GeneCreators
{
    public class GeneratorInitialGeneCreator : BaseInitialGeneCreator<GeneratorGene, GeneratorProduction>
    {
        protected override GeneratorGene CreateNewGene(GeneratorProduction entity)
        {
            return new GeneratorGene(entity.GlobalId, entity.ActivePower)
            {
                IsEnergized = random.Next(0, 2) == 1 ? true : false
            };
        }
    }
}
