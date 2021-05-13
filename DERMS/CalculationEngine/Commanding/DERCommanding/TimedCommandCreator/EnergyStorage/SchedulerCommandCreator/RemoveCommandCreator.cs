using CalculationEngine.Commanding.Commands;
using CalculationEngine.DERStates.CommandScheduler.Commands;

namespace CalculationEngine.Commanding.DERCommanding.TimedCommandCreator.EnergyStorage.SchedulerCommandCreator
{
    public class RemoveCommandCreator : ISchedulerCommandCreator
    {
        public SchedulerCommand CreateSchedulerCommand(Command command)
        {
            RemoveCommand removeCommand = new RemoveCommand(command.GlobalId);

            return removeCommand;
        }
    }
}
