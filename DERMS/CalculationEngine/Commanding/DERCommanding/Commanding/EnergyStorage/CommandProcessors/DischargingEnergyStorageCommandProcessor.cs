using CalculationEngine.Commanding.Commands;
using Common.DataTransferObjects.CalculationEngine;

namespace CalculationEngine.Commanding.DERCommanding.Commanding.EnergyStorage.CommandProcessors
{
    public class DischargingEnergyStorageCommandProcessor : BaseEnergyStorageCommandProcessor
    {
        private float lowerBoundStateOfCharge = 0.2f;

        public override Command CreateCommand(Model.DERCommanding.EnergyStorage energyStorage, float activePower)
        {
            EnergyStorageDischargeCommand dischargeCommand = new EnergyStorageDischargeCommand()
            {
                GlobalId = energyStorage.ActivePowerMeasurementGid,
                ActivePower = activePower,
                CommandFeedback = CreateCommandFeedback(energyStorage, activePower)
            };

            float capacity = energyStorage.Capacity * (energyStorage.StateOfCharge - lowerBoundStateOfCharge);
            dischargeCommand.SecondsOfUse = CalculateSecondsOfStorageUsage(capacity, activePower);

            return dischargeCommand;
        }

        private CommandFeedback CreateCommandFeedback(Model.DERCommanding.EnergyStorage energyStorage, float activePower)
        {
            CommandFeedback commandFeedback = new CommandFeedback();

            commandFeedback.Successful = true;
            commandFeedback.Message = $"Energy storage is now discharging with '{activePower}' active power.";

            return commandFeedback;
        }
    }
}
