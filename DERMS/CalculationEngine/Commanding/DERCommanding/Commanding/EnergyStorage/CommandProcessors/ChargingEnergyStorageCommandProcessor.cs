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

            float capacity = energyStorage.Capacity * (upperStateOfCharge - energyStorage.StateOfCharge);

            double secondsOfUse = CalculateSecondsOfStorageUsage(capacity, activePower);
            dischargeCommand.SecondsOfUse = secondsOfUse;

            return dischargeCommand;
        }

        private CommandFeedback CreateCommandFeedback(Model.DERCommanding.EnergyStorage energyStorage, float activePower)
        {
            CommandFeedback commandFeedback = new CommandFeedback();

            commandFeedback.Successful = true;
            commandFeedback.Message = $"Energy storage is now charging with '{-activePower}' active power.";

            return commandFeedback;
        }
    }
}
