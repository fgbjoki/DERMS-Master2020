using System.Collections.Generic;
using Common.AbstractModel;
using Common.ComponentStorage;
using Common.GDA;
using CalculationEngine.Model.EnergyCalculations;

namespace CalculationEngine.TransactionProcessing.StorageItemCreators.EnergyBalance
{
    public class EnergySourceStorageItemCreator : BaseEnergyBalanceStorageItemCreator
    {
        public EnergySourceStorageItemCreator() : base(PopulateNeededProperties())
        {
        }

        public override IdentifiedObject CreateStorageItem(ResourceDescription rd, Dictionary<DMSType, List<ResourceDescription>> affectedEntities)
        {
            EnergySource energySource = new EnergySource(rd.Id);

            if (!PopulateCalculationObjectWithActivePower(rd, energySource, affectedEntities))
            {
                return null;
            }

            return energySource;
        }

        private static Dictionary<DMSType, List<ModelCode>> PopulateNeededProperties()
        {
            return new Dictionary<DMSType, List<ModelCode>>()
            {
                { DMSType.ENERGYSOURCE, new List<ModelCode>()     { ModelCode.PSR_MEASUREMENTS } },
                { DMSType.MEASUREMENTANALOG, new List<ModelCode>()  { ModelCode.MEASUREMENT_MEASUREMENTYPE, ModelCode.MEASUREMENTANALOG_CURRENTVALUE } }
            };
        }
    }
}
