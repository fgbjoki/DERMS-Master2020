using CalculationEngine.Commanding.Commands;

namespace CalculationEngine.DERStates.CommandScheduler
{
    public interface ICommandScheduler
    {
        void ScheduleCommand(TimedCommand timedCommand);

        void RemoveScheduledCommand(long commandId);

        void PauseScheduledCommand(long commandId);

        void ResumeScheduledCommand(long commandId);
    }
}