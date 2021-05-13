namespace CalculationEngine.DERStates.CommandScheduler.Commands
{
    public class PauseCommand : SchedulerCommand
    {
        public PauseCommand(long commandId)
        {
            CommandId = commandId;
        }

        public long CommandId { get; private set; }

        public override void ExecuteCommand(ICommandScheduler commandScheduler)
        {
            commandScheduler.PauseScheduledCommand(CommandId);
        }
    }
}
