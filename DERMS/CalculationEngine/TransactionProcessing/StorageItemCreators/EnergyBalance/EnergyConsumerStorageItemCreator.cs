using System.Collections.Generic;
using Common.AbstractModel;
using Common.ComponentStorage;
using Common.GDA;
using CalculationEngine.Model.EnergyCalculations;
using Common.Logger;

namespace CalculationEngine.TransactionProcessing.StorageItemCreators.EnergyBalance
{
    public class EnergyConsumerStorageItemCreator : BaseEnergyBalanceStorageItemCreator
    {
        public EnergyConsumerStorageItemCreator() : base(PopulateNeededProperties())
        {
        }

        public override IdentifiedObject CreateStorageItem(ResourceDescription rd, Dictionary<DMSType, List<ResourceDescription>> affectedEntities)
        {
            EnergyConsumer energyConsumer = new EnergyConsumer(rd.Id);

            if (!PopulateCalculationObjectWithActivePower(rd, energyConsumer, affectedEntities))
            {
                return null;
            }

            return energyConsumer;
        }

        private static Dictionary<DMSType, List<ModelCode>> PopulateNeededProperties()
        {
            return new Dictionary<DMSType, List<ModelCode>>()
            {
                { DMSType.ENERGYCONSUMER, new List<ModelCode>()     { ModelCode.PSR_MEASUREMENTS } },
                { DMSType.MEASUREMENTANALOG, new List<ModelCode>()  { ModelCode.MEASUREMENT_MEASUREMENTYPE, ModelCode.MEASUREMENTANALOG_CURRENTVALUE } }
            };
        }
    }
}
