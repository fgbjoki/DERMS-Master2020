using CalculationEngine.Model.EnergyCalculations;
using Common.ComponentStorage;
using System.Collections.Generic;
using Common.AbstractModel;
using Common.ComponentStorage.StorageItemCreator;
using Common.GDA;

namespace CalculationEngine.TransactionProcessing.StorageTransactionProcessor.EnergyBalance
{
    public class EnergyBalanceStorageTransactionProcessor : StorageTransactionProcessor<CalculationObject>
    {
        public EnergyBalanceStorageTransactionProcessor(IStorage<CalculationObject> storage, Dictionary<DMSType, IStorageItemCreator> storageItemCreators) : base(storage, storageItemCreators)
        {
        }

        protected override List<DMSType> GetPrimaryTypes()
        {
            return new List<DMSType>(5) { DMSType.ENERGYCONSUMER, DMSType.ENERGYSTORAGE, DMSType.SOLARGENERATOR, DMSType.WINDGENERATOR, DMSType.ENERGYSOURCE };
        }

        protected override void AddAdditionalEntities(Dictionary<DMSType, List<long>> insertedEntities, Dictionary<DMSType, HashSet<long>> newNeededGids)
        {
            GDAProxy gdaProxy = new GDAProxy("gdaQueryEndpoint");

            if (!ShouldAddAdditionalEntities(insertedEntities))
            {
                return;
            }

            List<ResourceDescription> rds = gdaProxy.GetExtentValues(ModelCode.WINDGENERATOR, new List<ModelCode>() { ModelCode.PSR_MEASUREMENTS });
            rds.AddRange(gdaProxy.GetExtentValues(ModelCode.SOLARGENERATOR, new List<ModelCode>() { ModelCode.PSR_MEASUREMENTS }));
            rds.AddRange(gdaProxy.GetExtentValues(ModelCode.ENERGYSTORAGE, new List<ModelCode>() { ModelCode.PSR_MEASUREMENTS }));
            rds.AddRange(gdaProxy.GetExtentValues(ModelCode.ENERGYCONSUMER, new List<ModelCode>() { ModelCode.PSR_MEASUREMENTS }));
            rds.AddRange(gdaProxy.GetExtentValues(ModelCode.ENERGYSOURCE, new List<ModelCode>() { ModelCode.PSR_MEASUREMENTS }));

            if (rds?.Count == 0)
            {
                return;
            }

            foreach (var rd in rds)
            {
                List<long> measurements = rd.GetProperty(ModelCode.PSR_MEASUREMENTS).AsReferences();

                if (measurements.Count == 0)
                {
                    continue;
                }

                foreach (var measurement in measurements)
                {
                    DMSType dmsType = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(measurement);

                    if (dmsType != DMSType.MEASUREMENTANALOG)
                    {
                        continue;
                    }

                    if (!newNeededGids.ContainsKey(dmsType))
                    {
                        newNeededGids[dmsType] = new HashSet<long>();
                    }

                    newNeededGids[dmsType].Add(measurement);
                }           
            }

            base.AddAdditionalEntities(insertedEntities, newNeededGids);
        }

        private bool ShouldAddAdditionalEntities(Dictionary<DMSType, List<long>> insertedEntities)
        {
            return insertedEntities.ContainsKey(DMSType.WINDGENERATOR) || 
                insertedEntities.ContainsKey(DMSType.SOLARGENERATOR) || 
                insertedEntities.ContainsKey(DMSType.ENERGYSTORAGE) || 
                insertedEntities.ContainsKey(DMSType.ENERGYCONSUMER) ||
                insertedEntities.ContainsKey(DMSType.ENERGYSOURCE);
        }
    }
}
