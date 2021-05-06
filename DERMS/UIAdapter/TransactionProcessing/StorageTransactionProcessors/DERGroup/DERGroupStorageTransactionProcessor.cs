using Common.ComponentStorage;
using System.Collections.Generic;
using Common.AbstractModel;
using Common.ComponentStorage.StorageItemCreator;
using Common.GDA;

namespace UIAdapter.TransactionProcessing.StorageTransactionProcessors.DERGroup
{
    public class DERGroupStorageTransactionProcessor : StorageTransactionProcessor<Model.DERGroup.DERGroup>
    {
        public DERGroupStorageTransactionProcessor(IStorage<Model.DERGroup.DERGroup> storage, Dictionary<DMSType, IStorageItemCreator> storageItemCreators) : base(storage, storageItemCreators)
        {
        }

        protected override List<DMSType> GetPrimaryTypes()
        {
            return new List<DMSType>(1) { DMSType.ENERGYSTORAGE };
        }

        protected override void AddAdditionalEntities(Dictionary<DMSType, List<long>> insertedEntities, Dictionary<DMSType, HashSet<long>> newNeededGids)
        {
            GDAProxy gdaProxy = new GDAProxy("gdaQueryEndpoint");

            List<ResourceDescription> rds = gdaProxy.GetExtentValues(ModelCode.ENERGYSTORAGE, new List<ModelCode>() { ModelCode.PSR_MEASUREMENTS, ModelCode.ENERGYSTORAGE_GENERATOR });

            if (rds?.Count == 0)
            {
                return;
            }

            AddNewEntities(rds, newNeededGids);

            base.AddAdditionalEntities(insertedEntities, newNeededGids);
        }

        private void AddNewEntities(List<ResourceDescription> rds, Dictionary<DMSType, HashSet<long>> newNeededGids)
        {
            foreach (var rd in rds)
            {
                ProcessMeasurement(rd, newNeededGids);
                ProcessGenerators(rd, newNeededGids);
            }
        }

        private void ProcessMeasurement(ResourceDescription rd, Dictionary<DMSType, HashSet<long>> newNeededGids)
        {
            List<long> measurements = rd.GetProperty(ModelCode.PSR_MEASUREMENTS).AsReferences();

            if (measurements.Count == 0)
            {
                return;
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

        private void ProcessGenerators(ResourceDescription rd, Dictionary<DMSType, HashSet<long>> newNeededGids)
        {
            long generatorGid = rd.GetProperty(ModelCode.ENERGYSTORAGE_GENERATOR).AsReference();
            if (generatorGid == 0)
            {
                return;
            }

            DMSType dmsType = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(generatorGid);

            if (!newNeededGids.ContainsKey(dmsType))
            {
                newNeededGids[dmsType] = new HashSet<long>();
            }

            newNeededGids[dmsType].Add(generatorGid);
        }
    }
}
