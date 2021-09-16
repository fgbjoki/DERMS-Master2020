namespace CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Helpers
{
    public class EnergyStorageActivePowerCalculator
    {
        private readonly float lowerBoundStateOfCharge = 0.2f;
        private readonly float upperBoundStateOfCharge = 1f;

        public ulong SimulationInterval { get; set; }

        public float GetMaximumActivePower(float capacity, float stateOfCharge, float nominalPower)
        {
            float maximumActivePower = capacity * 3600 * (stateOfCharge - lowerBoundStateOfCharge);

            return maximumActivePower / SimulationInterval > nominalPower ? nominalPower : maximumActivePower;
        }

        public float GetMinimumActivePower(float capacity, float stateOfCharge, float nominalPower)
        {
            // maksimalnsa snaga za jedan sekund
            float minimumActivePower = capacity * 3600 * (upperBoundStateOfCharge - stateOfCharge);

            // ukupna aktivna snaga podeljena na 15 min ravnomerne potrosnje
            return minimumActivePower / SimulationInterval > nominalPower ? -nominalPower : -minimumActivePower;
        }
    }
}
