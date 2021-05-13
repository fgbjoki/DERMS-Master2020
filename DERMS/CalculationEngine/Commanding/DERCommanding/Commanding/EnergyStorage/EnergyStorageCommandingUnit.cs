using CalculationEngine.Commanding.Commands;
using CalculationEngine.Model.DERCommanding;
using Common.ComponentStorage;
using CalculationEngine.Commanding.DERCommanding.Commanding.EnergyStorage.CommandProcessors;

namespace CalculationEngine.Commanding.DERCommanding.Commanding.EnergyStorage
{
    public class EnergyStorageCommandingUnit : BaseCommandingUnit<Model.DERCommanding.EnergyStorage>
    {
        private BaseEnergyStorageCommandProcessor idleProcessor;
        private BaseEnergyStorageCommandProcessor chargingProcessor;
        private BaseEnergyStorageCommandProcessor dischargingProcessor;

        public EnergyStorageCommandingUnit(IStorage<DistributedEnergyResource> storage) : base(storage)
        {
            idleProcessor = new IdleStateEnergyStorageCommandProcessor();
            chargingProcessor = new ChargingEnergyStorageCommandProcessor();
            dischargingProcessor = new DischargingEnergyStorageCommandProcessor();
        }

        protected override Command CreateCommand(Model.DERCommanding.EnergyStorage energyStorage, float commandingValue)
        {
            BaseEnergyStorageCommandProcessor commandProcessor = GetCommandingProcessor(energyStorage, commandingValue);

            return commandProcessor.CreateCommand(energyStorage, commandingValue);
        }

        private BaseEnergyStorageCommandProcessor GetCommandingProcessor(Model.DERCommanding.EnergyStorage energyStorage, float commandingValue)
        {
            if (commandingValue > 0)
            {
                return dischargingProcessor;
            }
            else if (commandingValue < 0)
            {
                return chargingProcessor;
            }
            else
            {
                return idleProcessor;
            }
        }

        private void InitializeProcessors()
        {
            idleProcessor = new IdleStateEnergyStorageCommandProcessor();
            chargingProcessor = new ChargingEnergyStorageCommandProcessor();
            dischargingProcessor = new DischargingEnergyStorageCommandProcessor();
        }
    }
}
