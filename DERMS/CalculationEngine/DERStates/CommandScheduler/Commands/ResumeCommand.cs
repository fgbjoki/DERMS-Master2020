namespace CalculationEngine.DERStates.CommandScheduler.Commands
{
    public class ResumeCommand : SchedulerCommand
    {
        public ResumeCommand(long commandId)
        {
            CommandId = commandId;
        }

        public long CommandId { get; private set; }

        public override void ExecuteCommand(ICommandScheduler commandScheduler)
        {
            commandScheduler.RemoveScheduledCommand(CommandId);
        }
    }
}
