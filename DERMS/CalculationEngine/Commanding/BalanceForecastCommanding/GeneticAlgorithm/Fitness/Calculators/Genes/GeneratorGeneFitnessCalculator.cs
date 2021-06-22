using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Model.Genes;

namespace CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Fitness.Calculators.Genes
{
    public class GeneratorGeneFitnessCalculator : BaseDERGeneFitnessCalculator<GeneratorGene>
    {
        protected override float Calculate(GeneratorGene gene, DomainParameters domainParameters)
        {
            return gene.IsEnergized ? 0 : gene.ActivePower * domainParameters.SimulationInterval / 3600 * domainParameters.FitnessParameters.CostOfGeneratorShutdownPerKWH;
        }
    }
}
