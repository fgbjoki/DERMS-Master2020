using CalculationEngine.Commanding.Commands;
using CalculationEngine.DERStates.CommandScheduler.Commands;

namespace CalculationEngine.Commanding.DERCommanding.TimedCommandCreator
{
    public interface ISchedulerCommandCreator
    {
        /// <summary>
        /// Creates timed command based on <paramref name="command"/>.
        /// </summary>
        SchedulerCommand CreateSchedulerCommand(Command command);
    }
}
