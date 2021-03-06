﻿using FieldSimulator.PowerSimulator.Storage;
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
        private float maximumCapacity;

        public EnergyStorageStateOfChargeCalculation(float nominalPower, float capacity)
        {
            this.nominalPower = nominalPower;

            // power * time = energy
            maximumCapacity = capacity * 3600;
        }

        public override void Calculate(PowerGridSimulatorStorage powerGridSimulatorStorage, double simulationInterval)
        {
            float activePower = GetActivePower(powerGridSimulatorStorage);

            float currentCapacity = maximumCapacity * GetStateOfCharge(powerGridSimulatorStorage);

            float leftOverEnergy = currentCapacity - (activePower * Convert.ToSingle(simulationInterval));

            float percentageEnergyLeft = (leftOverEnergy / maximumCapacity) * 100;

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
