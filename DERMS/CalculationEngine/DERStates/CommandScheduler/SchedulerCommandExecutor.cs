using CalculationEngine.DERStates.CommandScheduler.Commands;

namespace CalculationEngine.DERStates.CommandScheduler
{
    public class SchedulerCommandExecutor : ISchedulerCommandExecutor
    {
        private ICommandScheduler commandScheduler;

        public SchedulerCommandExecutor()
        {
        }

        public void ExecuteCommand(SchedulerCommand schedulerCommand)
        {
            schedulerCommand.ExecuteCommand(commandScheduler);
        }

        public void SetCommandScheduler(ICommandScheduler commandScheduler)
        {
            this.commandScheduler = commandScheduler;
        }
    }
}
