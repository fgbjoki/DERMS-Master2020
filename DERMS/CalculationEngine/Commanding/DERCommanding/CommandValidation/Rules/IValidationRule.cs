using CalculationEngine.Model.DERCommanding;
using Common.DataTransferObjects.CalculationEngine;

namespace CalculationEngine.Commanding.DERCommanding.CommandValidation.Rules
{
    public interface IValidationRule
    {
        CommandFeedback IsCommandingValid(DistributedEnergyResource commandingEntity, float commandingValue);
    }
}