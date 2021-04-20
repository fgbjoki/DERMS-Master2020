using FieldSimulator.PowerSimulator.Storage;
using System;

namespace FieldSimulator.PowerSimulator.Calculations
{
    /// Input parameter indexes:
    /// 0 - ActivePower
    /// 
    /// Output parameter indexes:
    /// 0 - StateOfCharge
    class EnergyStorageStateOfChargeCalculation : Calculation
    {
        /// <summary>
        /// W.
        /// </summary>
        private float nominalPower;

        /// <summary>
        /// Wh.
        /// </summary>
        private float capacity;

        private float maximumCapacityPower;

        public EnergyStorageStateOfChargeCalculation(float nominalPower, float capacity)
        {
            this.capacity = capacity;
            this.nominalPower = nominalPower;

            maximumCapacityPower = capacity * 3600;
        }

        public override void Calculate(PowerGridSimulatorStorage powerGridSimulatorStorage, double simulationInterval)
        {
            float activePower = GetActivePower(powerGridSimulatorStorage);

            float currentCapacity = maximumCapacityPower * GetStateOfCharge(powerGridSimulatorStorage);

            float leftOverEnergy = currentCapacity - (activePower * Convert.ToSingle(simulationInterval));

            float percentageEnergyLeft = leftOverEnergy * 100 / maximumCapacityPower;

            ChangeStateOfCharge(powerGridSimulatorStorage, percentageEnergyLeft);
        }

        private float GetActivePower(IRemotePointStorageStorage<float> analogPoints)
        {
            CalculationParameter activePowerParamter = inputs[0];
            return analogPoints.GetValue(activePowerParamter.RemotePointType, activePowerParamter.Address);
        }

        private float GetStateOfCharge(IRemotePointStorageStorage<float> analogPoints)
        {
            CalculationParameter stateOfChargeParameter = outputs[0];
            return analogPoints.GetValue(stateOfChargeParameter.RemotePointType, stateOfChargeParameter.Address);
        }

        private void ChangeStateOfCharge(IRemotePointStorageStorage<float> analogPoints, float percentageEnergyUsed)
        {
            CalculationParameter stateOfChargeParameter = outputs[0];
            float currentStateOfCharge = analogPoints.GetValue(stateOfChargeParameter.RemotePointType, stateOfChargeParameter.Address);

            currentStateOfCharge = percentageEnergyUsed / 100;

            analogPoints.UpdateValue(stateOfChargeParameter.RemotePointType, stateOfChargeParameter.Address, currentStateOfCharge);
        }
    }
}
