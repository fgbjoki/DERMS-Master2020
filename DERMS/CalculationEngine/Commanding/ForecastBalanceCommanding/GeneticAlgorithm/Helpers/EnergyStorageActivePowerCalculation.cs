using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Helpers.FitnessParameters;

namespace CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Helpers
{
    public class EnergyStorageActivePowerCalculation
    {
        private BoundaryParameteres boundParameters;

        public EnergyStorageActivePowerCalculation(BoundaryParameteres boundParameters)
        {
            this.boundParameters = boundParameters;
        }

        public float GetMaximumActivePower(float capacity, float stateOfCharge, float nominalPower)
        {
            float maximumActivePower = capacity * 3600 * (stateOfCharge - boundParameters.LowerBoundStateOfCharge) / boundParameters.SimulationInterval;

            return maximumActivePower > nominalPower ? nominalPower : maximumActivePower;
        }

        public float GetMinimumActivePower(float capacity, float stateOfCharge, float nominalPower)
        {
            float minimumActivePower = capacity * 3600 * (boundParameters.UpperBoundStateOfCharge - stateOfCharge);

            return minimumActivePower > nominalPower * boundParameters.SimulationInterval ? -nominalPower : -minimumActivePower;
        }
    }
}
