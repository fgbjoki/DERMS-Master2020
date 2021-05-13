using CalculationEngine.Commanding.DERCommanding.CommandValidation.Rules;
using Common.DataTransferObjects.CalculationEngine;
using System;

namespace CalculationEngine.Commanding.DERCommanding.CommandValidation.EnergyStorage.Rules
{
    /// <summary>
    /// Restricts situation where active power is larger than nominal power.
    /// </summary>
    public class NominalPowerRestrictionRule : BaseValidationRule<Model.DERCommanding.EnergyStorage>
    {
        public override CommandFeedback IsCommandingValid(Model.DERCommanding.EnergyStorage commandingEntity, float commandingValue)
        {
            CommandFeedback validationFeedback = new CommandFeedback();
            validationFeedback.Successful = commandingEntity.NominalPower >= Math.Abs(commandingValue);
            if (!validationFeedback.Successful)
            {
                validationFeedback.Message = "Specified active power exceeds nominal power";
            }

            return validationFeedback;
        }
    }
}
