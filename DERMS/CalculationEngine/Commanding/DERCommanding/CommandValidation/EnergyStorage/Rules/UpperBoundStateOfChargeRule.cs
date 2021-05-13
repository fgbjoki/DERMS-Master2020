using CalculationEngine.Commanding.DERCommanding.CommandValidation.Rules;
using Common.DataTransferObjects.CalculationEngine;

namespace CalculationEngine.Commanding.DERCommanding.CommandValidation.EnergyStorage.Rules
{
    public class UpperBoundStateOfChargeRule : BaseValidationRule<Model.DERCommanding.EnergyStorage>
    {
        private float upperBoundStateOfCharge;

        public UpperBoundStateOfChargeRule(float upperBoundStateOfCharge)
        {
            this.upperBoundStateOfCharge = upperBoundStateOfCharge;
        }
        public override CommandFeedback IsCommandingValid(Model.DERCommanding.EnergyStorage energyStorage, float activePower)
        {
            CommandFeedback validationFeedback = new CommandFeedback();
            validationFeedback.Successful = energyStorage.StateOfCharge < upperBoundStateOfCharge;

            if (!validationFeedback.Successful)
            {
                validationFeedback.Message = "Energy storage is full.";
            }

            return validationFeedback;
        }
    }
}
