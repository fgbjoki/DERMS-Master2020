using CalculationEngine.Commanding.Commands;
using CalculationEngine.DERStates.CommandScheduler.Commands;

namespace CalculationEngine.Commanding.DERCommanding.TimedCommandCreator.EnergyStorage.SchedulerCommandCreator
{
    public class IdleStateCommandCreator : ISchedulerCommandCreator
    {
        public SchedulerCommand CreateSchedulerCommand(Command command)
        {
            AddCommand addCommand = new AddCommand(CreateTimedCommand(command));

            return addCommand;
        }

        private TimedCommand CreateTimedCommand(Command command)
        {
            BaseEnergyStorageCommand energyStorageCommand = command as BaseEnergyStorageCommand;

            return new TimedCommand(energyStorageCommand.SecondsOfUse * 100, new EnergyStorageIdleStateCommand() { GlobalId = energyStorageCommand.GlobalId });
        }
    }
}
