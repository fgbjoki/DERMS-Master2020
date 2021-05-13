using CalculationEngine.Commanding.Commands;
using Common.DataTransferObjects.CalculationEngine;

namespace CalculationEngine.Commanding.DERCommanding.Commanding.EnergyStorage.CommandProcessors
{
    public class ChargingEnergyStorageCommandProcessor : BaseEnergyStorageCommandProcessor
    {
        private float upperStateOfCharge = 1f;

        public override Command CreateCommand(Model.DERCommanding.EnergyStorage energyStorage, float activePower)
        {
            EnergyStorageChargeCommand dischargeCommand = new EnergyStorageChargeCommand()
            {
                GlobalId = energyStorage.ActivePowerMeasurementGid,
                ActivePower = activePower,
                CommandFeedback = CreateCommandFeedback(energyStorage, activePower)
            };

            return dischargeCommand;
        }

        private CommandFeedback CreateCommandFeedback(Model.DERCommanding.EnergyStorage energyStorage, float activePower)
        {
            CommandFeedback commandFeedback = new CommandFeedback();
            double secondsOfUse = CalculateSecondsOfCharging(energyStorage.Capacity, energyStorage.StateOfCharge, activePower);

            commandFeedback.Successful = true;
            commandFeedback.Message = $"Energy storage is now charging with '{-activePower}' active power.";

            return commandFeedback;
        }

        private double CalculateSecondsOfCharging(float capacity, float stateOfCharge, float commandedActivePower)
        {
            double capacityToCharge = capacity * (upperStateOfCharge - stateOfCharge);
            double activePowerToCharge = capacityToCharge * 3600;

            double secondsOfUse = capacityToCharge / commandedActivePower;

            return secondsOfUse;
        }
    }
}
