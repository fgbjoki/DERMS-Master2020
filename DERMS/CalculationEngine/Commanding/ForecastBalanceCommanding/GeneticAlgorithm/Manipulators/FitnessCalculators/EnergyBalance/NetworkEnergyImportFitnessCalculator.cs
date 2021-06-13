namespace CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Manipulators.FitnessCalculators.EnergyBalance
{
    public class NetworkEnergyImportFitnessCalculator
    {
        public ulong SimulationInterval { get; set; }

        public float KWHCost { get; set; }

        public double Calculate(float demand, float response)
        {
            float energyToImport = demand - response;
            if (energyToImport < 0)
            {
                return double.PositiveInfinity;
            }

            return energyToImport * SimulationInterval / 3600 * KWHCost;
        }
    }
}
