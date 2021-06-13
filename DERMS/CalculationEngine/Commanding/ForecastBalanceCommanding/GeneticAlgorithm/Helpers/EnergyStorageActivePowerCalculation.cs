namespace CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Helpers
{
    public class EnergyStorageActivePowerCalculation
    {
        private float lowerBoundStateOfCharge;
        private float upperBoundStateOfCharge;

        private ulong intervalSimulation;

        public EnergyStorageActivePowerCalculation(float lowerBoundStateOfCharge, float upperBoundStateOfCharge, ulong intervalSimulation)
        {
            this.lowerBoundStateOfCharge = lowerBoundStateOfCharge;
            this.upperBoundStateOfCharge = upperBoundStateOfCharge;
            this.intervalSimulation = intervalSimulation;
        }

        public float GetMaximumActivePower(float capacity, float stateOfCharge, float nominalPower)
        {
            float maximumActivePower = capacity * 3600 * (stateOfCharge - lowerBoundStateOfCharge) / intervalSimulation;

            return maximumActivePower > nominalPower ? nominalPower : maximumActivePower;
        }

        public float GetMinimumActivePower(float capacity, float stateOfCharge, float nominalPower)
        {
            float minimumActivePower = capacity * 3600 * (upperBoundStateOfCharge - stateOfCharge);

            return minimumActivePower > nominalPower * intervalSimulation ? -nominalPower : -minimumActivePower;
        }
    }
}
