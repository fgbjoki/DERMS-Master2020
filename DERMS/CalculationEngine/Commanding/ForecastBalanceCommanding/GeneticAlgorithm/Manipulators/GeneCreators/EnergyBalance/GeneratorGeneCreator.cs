using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Model.EnergyBalance.Genes;

namespace CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Manipulators.GeneCreators.EnergyBalance
{
    public class GeneratorGeneCreator : BaseGeneCreator<GeneratorGene>
    {
        public override void Crossover(GeneratorGene child, GeneratorGene firstParent, GeneratorGene secondParent)
        {
            int parentGene = random.Next(0, 1);

            child.IsEnergized = parentGene == 1 ? firstParent.IsEnergized : secondParent.IsEnergized;
        }
    }
}
