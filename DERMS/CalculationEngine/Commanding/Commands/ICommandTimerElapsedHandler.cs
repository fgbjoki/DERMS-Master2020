using System.Timers;

namespace CalculationEngine.Commanding.Commands
{
    public interface ICommandTimerElapsedHandler
    {
        void CommandTimerElapsed(Command command);
    }
}