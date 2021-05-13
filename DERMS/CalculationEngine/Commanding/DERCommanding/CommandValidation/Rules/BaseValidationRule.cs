using CalculationEngine.Model.DERCommanding;
using Common.DataTransferObjects.CalculationEngine;

namespace CalculationEngine.Commanding.DERCommanding.CommandValidation.Rules
{
    public abstract class BaseValidationRule<T> : IValidationRule
        where T : DistributedEnergyResource
    {
        public CommandFeedback IsCommandingValid(DistributedEnergyResource commandingEntity, float commandingValue)
        {
            return IsCommandingValid(commandingEntity as T, commandingValue);
        }

        public abstract CommandFeedback IsCommandingValid(T commandingEntity, float commandingValue);
    }
}
