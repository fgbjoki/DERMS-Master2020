namespace CalculationEngine.DERStates.CommandScheduler.Commands
{
    public abstract class SchedulerCommand
    {
        public abstract void ExecuteCommand(ICommandScheduler commandScheduler);
    }
}
