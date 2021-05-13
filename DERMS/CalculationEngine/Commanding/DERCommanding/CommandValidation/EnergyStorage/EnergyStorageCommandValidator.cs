using CalculationEngine.Model.DERCommanding;
using Common.ComponentStorage;
using Common.ServiceInterfaces.CalculationEngine;
using CalculationEngine.Commanding.DERCommanding.CommandValidation.Rules;
using CalculationEngine.Commanding.DERCommanding.CommandValidation.EnergyStorage.Rules.ComplexRules;
using Common.DataTransferObjects.CalculationEngine;

namespace CalculationEngine.Commanding.DERCommanding.CommandValidation.EnergyStorage
{
    public class EnergyStorageCommandValidator : BaseCommandingValidator<Model.DERCommanding.EnergyStorage>
    {
        private BaseValidationRule<Model.DERCommanding.EnergyStorage> chargingRule;
        private BaseValidationRule<Model.DERCommanding.EnergyStorage> discharginRule;

        public EnergyStorageCommandValidator(IDERStateDeterminator derStateDeterminator, IStorage<DistributedEnergyResource> DERStorage) : base(derStateDeterminator, DERStorage)
        {
            InitializeRules(derStateDeterminator);
        }

        protected override CommandFeedback ValidateCommand(Model.DERCommanding.EnergyStorage energyStorage, float activePower)
        {
            BaseValidationRule<Model.DERCommanding.EnergyStorage> rule = null;

            if (activePower == 0)
            {
                CommandFeedback commandFeedback = new CommandFeedback();
                commandFeedback.Successful = true;
                commandFeedback.Message = "Energy storage will go into Idle state.";

                return commandFeedback;
            }
            else if (activePower > 0)
            {
                rule = discharginRule;
            }
            else
            {
                rule = chargingRule;
            }

            return rule.IsCommandingValid(energyStorage, activePower);
        }

        private void InitializeRules(IDERStateDeterminator derStateDeterminator)
        {
            chargingRule = new EnergyStorageChargingRule(derStateDeterminator);
            discharginRule = new EnergyStorageDischargingRule(derStateDeterminator);
        }
    }
}
