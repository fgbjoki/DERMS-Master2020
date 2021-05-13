using CalculationEngine.Commanding.Commands;
using Common.DataTransferObjects.CalculationEngine;

namespace CalculationEngine.Commanding.DERCommanding.Commanding.EnergyStorage.CommandProcessors
{
    public class IdleStateEnergyStorageCommandProcessor : BaseEnergyStorageCommandProcessor
    {
        public override Command CreateCommand(Model.DERCommanding.EnergyStorage energyStorage, float activePower)
        {
            return new EnergyStorageIdleStateCommand()
            {
                GlobalId = energyStorage.ActivePowerMeasurementGid,
                CommandFeedback = new CommandFeedback()
                {
                    Successful = true,
                    Message = "Energy storage is in idle state."
                }
            };
        }
    }
}
