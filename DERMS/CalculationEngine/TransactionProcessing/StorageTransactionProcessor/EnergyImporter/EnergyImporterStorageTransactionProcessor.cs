using CalculationEngine.Model.EnergyImporter;
using Common.ComponentStorage;
using System.Collections.Generic;
using Common.AbstractModel;
using Common.ComponentStorage.StorageItemCreator;
using Common.GDA;

namespace CalculationEngine.TransactionProcessing.StorageTransactionProcessor.EnergyImporter
{
    public class EnergyImporterStorageTransactionProcessor : StorageTransactionProcessor<EnergySource>
    {
        public EnergyImporterStorageTransactionProcessor(IStorage<EnergySource> storage, Dictionary<DMSType, IStorageItemCreator> storageItemCreators) : base(storage, storageItemCreators)
        {
        }

        protected override List<DMSType> GetPrimaryTypes()
        {
            return new List<DMSType>(1) { DMSType.ENERGYSOURCE };
        }

        protected override void AddAdditionalEntities(Dictionary<DMSType, List<long>> insertedEntities, Dictionary<DMSType, HashSet<long>> newNeededGids)
        {
            GDAProxy gdaProxy = new GDAProxy("gdaQueryEndpoint");

            List<ResourceDescription> rds = gdaProxy.GetExtentValues(ModelCode.ENERGYSOURCE, new List<ModelCode>() { ModelCode.PSR_MEASUREMENTS });

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
    }
}
