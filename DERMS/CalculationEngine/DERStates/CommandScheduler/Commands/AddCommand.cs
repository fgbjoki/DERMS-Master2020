using CalculationEngine.Commanding.Commands;

namespace CalculationEngine.DERStates.CommandScheduler.Commands
{
    public class AddCommand : SchedulerCommand
    {
        public AddCommand(TimedCommand timedCommand)
        {
            TimedCommand = timedCommand;
        }

        public TimedCommand TimedCommand { get; private set; }

        public override void ExecuteCommand(ICommandScheduler commandScheduler)
        {
            commandScheduler.ScheduleCommand(TimedCommand);
        }
    }
}
