using System.Collections.Generic;
using Common.AbstractModel;
using Common.ComponentStorage;
using Common.GDA;
using CalculationEngine.Model.EnergyCalculations;
using Common.Logger;

namespace CalculationEngine.TransactionProcessing.StorageItemCreators.EnergyBalance
{
    public class EnergyGeneratorStorageItemCreator : BaseEnergyBalanceStorageItemCreator
    {
        public EnergyGeneratorStorageItemCreator() : base(PopulateNeededProperties())
        {
        }

        public override IdentifiedObject CreateStorageItem(ResourceDescription rd, Dictionary<DMSType, List<ResourceDescription>> affectedEntities)
        {
            EnergyGenerator energyGenerator = new EnergyGenerator(rd.Id);

            if (!PopulateCalculationObjectWithActivePower(rd, energyGenerator, affectedEntities))
            {
                return null;
            }

            return energyGenerator;
        }

        private static Dictionary<DMSType, List<ModelCode>> PopulateNeededProperties()
        {
            return new Dictionary<DMSType, List<ModelCode>>()
            {
                { DMSType.ENERGYSTORAGE, new List<ModelCode>()      { ModelCode.PSR_MEASUREMENTS } },
                { DMSType.SOLARGENERATOR, new List<ModelCode>()     { ModelCode.PSR_MEASUREMENTS } },
                { DMSType.WINDGENERATOR, new List<ModelCode>()      { ModelCode.PSR_MEASUREMENTS } },
                { DMSType.MEASUREMENTANALOG, new List<ModelCode>()  { ModelCode.MEASUREMENT_MEASUREMENTYPE, ModelCode.MEASUREMENTANALOG_CURRENTVALUE } }
            };
        }
    }
}
