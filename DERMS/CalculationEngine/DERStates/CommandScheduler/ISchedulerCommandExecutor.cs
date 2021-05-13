using CalculationEngine.DERStates.CommandScheduler.Commands;

namespace CalculationEngine.DERStates.CommandScheduler
{
    public interface ISchedulerCommandExecutor
    {
        void ExecuteCommand(SchedulerCommand schedulerCommand);
    }
}