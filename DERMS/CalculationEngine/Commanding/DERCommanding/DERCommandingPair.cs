using CalculationEngine.Commanding.Commands;
using CalculationEngine.Commanding.DERCommanding.Commanding;
using CalculationEngine.Commanding.DERCommanding.CommandValidation;
using Common.DataTransferObjects.CalculationEngine;

namespace CalculationEngine.Commanding.DERCommanding
{
    public class DERCommandingPair : IDERCommandCreator, IDERUnitCommandValidator
    {
        private IDERUnitCommandValidator validator;
        private IDERCommandCreator commandCreator;

        public DERCommandingPair(IDERUnitCommandValidator validator, IDERCommandCreator commandCreator)
        {
            this.validator = validator;
            this.commandCreator = commandCreator;
        }

        public CommandFeedback ValidateCommand(long derGid, float commandingValue)
        {
            return validator.ValidateCommand(derGid, commandingValue);
        }

        public Command CreateCommand(long derGid, float commandingValue)
        {
            return commandCreator.CreateCommand(derGid, commandingValue);
        }
    }
}
