using Common.DataTransferObjects.CalculationEngine;
using Common.ServiceInterfaces.NetworkDynamicsService.Commands;

namespace CalculationEngine.Commanding.Commands
{
    public abstract class Command
    {
        public long GlobalId { get; set; }

        public CommandFeedback CommandFeedback { get; set; }

        public virtual BaseCommand CreateNDSCommand()
        {
            return null;
        }

        public virtual TimedCommand CreateScheduleCommand(double millisecondsTime)
        {
            return new TimedCommand(millisecondsTime, this);
        }
    }
}
