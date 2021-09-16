namespace CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Fitness.Calculators.Chromosomes
{
    public class EnergyImportFitnessCalculator
    {
        public float Calculate(DomainParameters domainParameters, float response)
        {
            if (response > domainParameters.EnergyDemand)
            {
                return float.PositiveInfinity;
            }

            return (domainParameters.EnergyDemand - response) * domainParameters.SimulationInterval / 3600 * domainParameters.FitnessParameters.CostOfEnergyImportPerKWH;
        }
    }
}
