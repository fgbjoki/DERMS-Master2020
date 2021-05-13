using CalculationEngine.Commanding.Commands;
using CalculationEngine.Commanding.DERCommanding.Commanding;
using CalculationEngine.Commanding.DERCommanding.CommandValidation;
using CalculationEngine.Commanding.DERCommanding.TimedCommandCreator;
using CalculationEngine.DERStates.CommandScheduler.Commands;
using Common.DataTransferObjects.CalculationEngine;

namespace CalculationEngine.Commanding.DERCommanding
{
    public class DERCommandingWrapper : IDERCommandCreator, IDERUnitCommandValidator, ISchedulerCommandCreator
    {
        private IDERUnitCommandValidator validator;
        private IDERCommandCreator commandCreator;
        private ISchedulerCommandCreator timedCommandCreator;

        public DERCommandingWrapper(IDERUnitCommandValidator validator, IDERCommandCreator commandCreator, ISchedulerCommandCreator timedCommandCreator)
        {
            this.validator = validator;
            this.commandCreator = commandCreator;
            this.timedCommandCreator = timedCommandCreator;
        }

        public CommandFeedback ValidateCommand(long derGid, float commandingValue)
        {
            return validator.ValidateCommand(derGid, commandingValue);
        }

        public Command CreateCommand(long derGid, float commandingValue)
        {
            return commandCreator.CreateCommand(derGid, commandingValue);
        }

        public SchedulerCommand CreateSchedulerCommand(Command command)
        {
            return timedCommandCreator?.CreateSchedulerCommand(command);
        }
    }
}
