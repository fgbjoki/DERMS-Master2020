using Common.DataTransferObjects.CalculationEngine;

namespace CalculationEngine.Commanding.DERCommanding.CommandValidation
{
    public interface IDERUnitCommandValidator
    {
        CommandFeedback ValidateCommand(long derGid, float commandingValue);
    }
}