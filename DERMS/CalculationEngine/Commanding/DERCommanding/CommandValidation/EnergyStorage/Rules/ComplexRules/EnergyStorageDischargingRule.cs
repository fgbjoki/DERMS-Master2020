using CalculationEngine.Commanding.DERCommanding.CommandValidation.Rules;
using Common.DataTransferObjects.CalculationEngine;
using Common.ServiceInterfaces.CalculationEngine;
using System.Collections.Generic;

namespace CalculationEngine.Commanding.DERCommanding.CommandValidation.EnergyStorage.Rules.ComplexRules
{
    public class EnergyStorageDischargingRule : BaseEnergyStorageStateOfChargeRule
    {
        private List<IValidationRule> internalRules;

        public EnergyStorageDischargingRule(IDERStateDeterminator derstateDeterminator)
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

            float currentCapcity = energyStorage.Capacity * (energyStorage.StateOfCharge - 0.2f);
            commandFeedback.Message = $"Energy storage can discharge with specified active power for {CreateStringChargingTime(currentCapcity, activePower)}";

            return commandFeedback;
        }

        private void InitializeInternalRules(IDERStateDeterminator derstateDeterminator)
        {
            internalRules = new List<IValidationRule>()
            {
                new NominalPowerRestrictionRule(),
                new LowerBoundStateOfChargeRule(0.20f)
            };
        }
    }
}
