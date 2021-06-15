using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Model.EnergyBalance.Genes;

namespace CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Manipulators.Mutators.EnergyBalance
{
    public class GeneratorGeneMutator : BaseGeneMutator<GeneratorGene>
    {
        public override void Mutate(GeneratorGene gene)
        {
            gene.IsEnergized = random.Next(0, 2) == 1 ? true : false;
        }
    }
}
