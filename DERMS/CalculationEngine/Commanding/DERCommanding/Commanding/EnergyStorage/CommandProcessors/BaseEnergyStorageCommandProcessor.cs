using CalculationEngine.Commanding.Commands;
using System;

namespace CalculationEngine.Commanding.DERCommanding.Commanding.EnergyStorage.CommandProcessors
{
    public abstract class BaseEnergyStorageCommandProcessor
    {
        public abstract Command CreateCommand(Model.DERCommanding.EnergyStorage energyStorage, float activePower);

        protected double CalculateSecondsOfStorageUsage(float capacity, float commandedActivePower)
        {
            double activePowerCapacity = capacity * 3600;

            double secondsOfUse = activePowerCapacity / Math.Abs(commandedActivePower);

            return secondsOfUse;
        }
    }
}
