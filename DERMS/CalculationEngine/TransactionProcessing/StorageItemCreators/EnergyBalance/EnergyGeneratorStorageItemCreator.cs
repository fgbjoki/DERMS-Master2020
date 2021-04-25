using System.Collections.Generic;
using Common.AbstractModel;
using Common.ComponentStorage;
using Common.GDA;
using CalculationEngine.Model.EnergyCalculations;
using System;

namespace CalculationEngine.TransactionProcessing.StorageItemCreators.EnergyBalance
{
    public class EnergyGeneratorStorageItemCreator : BaseEnergyBalanceStorageItemCreator
    {
        public EnergyGeneratorStorageItemCreator() : base()
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

        public override Dictionary<DMSType, List<ModelCode>> GetNeededProperties()
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
