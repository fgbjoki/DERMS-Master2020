using System.Collections.Generic;
using Common.ServiceInterfaces.CalculationEngine;
using CalculationEngine.Commanding.DERCommanding.CommandValidation.Rules;
using Common.DataTransferObjects.CalculationEngine;

namespace CalculationEngine.Commanding.DERCommanding.CommandValidation.EnergyStorage.Rules.ComplexRules
{
    public class EnergyStorageChargingRule : BaseEnergyStorageStateOfChargeRule
    {
        private List<IValidationRule> internalRules;

        public EnergyStorageChargingRule(IDERStateDeterminator derstateDeterminator)
        {
            InitializeInternalRules(derstateDeterminator);
        }

        public override CommandFeedback IsCommandingValid(Model.DERCommanding.EnergyStorage energyStorage, float activePower)
        {
            CommandFeedback commandFeedback = null;
            foreach (var rule in internalRules)
            {
                commandFeedback = rule.IsCommandingValid(energyStorage, activePower);
                if (!commandFeedback.Successful)
                {
                    return commandFeedback;
                }
            }

            float currentCapcity = energyStorage.Capacity * (1f - energyStorage.StateOfCharge);
            commandFeedback.Message = $"Energy storage can be charged with specified active power for {CreateStringChargingTime(currentCapcity, activePower)}";

            return commandFeedback;
        }

        private void InitializeInternalRules(IDERStateDeterminator derstateDeterminator)
        {
            internalRules = new List<IValidationRule>()
            {
                new UpperBoundStateOfChargeRule(1f),
                new NominalPowerRestrictionRule()
            };
        }
    }
}
