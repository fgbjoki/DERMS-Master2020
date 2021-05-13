namespace CalculationEngine.DERStates.CommandScheduler.Commands
{
    public class RemoveCommand : SchedulerCommand
    {
        public RemoveCommand(long commandId)
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
