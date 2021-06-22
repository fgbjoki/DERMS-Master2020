using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Model.Genes;

namespace CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Fitness.Calculators.Genes
{
    public class EnergyStorageGeneFitnessCalculator : BaseDERGeneFitnessCalculator<EnergyStorageGene>
    {
        protected override float Calculate(EnergyStorageGene gene, DomainParameters domainParameters)
        {
            return gene.ActivePower * domainParameters.FitnessParameters.CostOfEnergyStorageUsePerKWH * domainParameters.SimulationInterval / 3600;
        }
    }
}
