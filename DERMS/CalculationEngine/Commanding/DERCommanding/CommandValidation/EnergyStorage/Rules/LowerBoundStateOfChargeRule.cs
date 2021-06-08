using CalculationEngine.Commanding.DERCommanding.CommandValidation.Rules;
using Common.DataTransferObjects.CalculationEngine;

namespace CalculationEngine.Commanding.DERCommanding.CommandValidation.EnergyStorage.Rules
{
    public class LowerBoundStateOfChargeRule : BaseValidationRule<Model.DERCommanding.EnergyStorage>
    {
        private float lowerBoundStateOfCharge;

        public LowerBoundStateOfChargeRule(float lowerBoundStateOfCharge)
        {
            this.lowerBoundStateOfCharge = lowerBoundStateOfCharge;
        }

        public override CommandFeedback IsCommandingValid(Model.DERCommanding.EnergyStorage energyStorage, float activePower)
        {
            CommandFeedback validationFeedback = new CommandFeedback();
            validationFeedback.Successful = energyStorage.StateOfCharge > lowerBoundStateOfCharge;
            if (!validationFeedback.Successful)
            {
                validationFeedback.Message = $"Cannot discharge energy storage which exceeds lower bound state of charge ({lowerBoundStateOfCharge * 100}%)!";
            }

            return validationFeedback;
        }
    }
}
