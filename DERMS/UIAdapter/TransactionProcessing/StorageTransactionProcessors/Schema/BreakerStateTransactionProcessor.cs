using Common.ComponentStorage;
using System.Collections.Generic;
using UIAdapter.Model.Schema;
using Common.AbstractModel;
using Common.ComponentStorage.StorageItemCreator;
using Common.GDA;
using System.Linq;

namespace UIAdapter.TransactionProcessing.StorageTransactionProcessors.Schema
{
    public class BreakerStateTransactionProcessor : StorageTransactionProcessor<Breaker>
    {
        public BreakerStateTransactionProcessor(IStorage<Breaker> storage, Dictionary<DMSType, IStorageItemCreator> storageItemCreators) : base(storage, storageItemCreators)
        {
        }

        protected override List<DMSType> GetPrimaryTypes()
        {
            return new List<DMSType>(1) { DMSType.BREAKER };
        }

        protected override void AddAdditionalEntities(Dictionary<DMSType, List<long>> insertedEntities, Dictionary<DMSType, HashSet<long>> newNeededGids)
        {
            GDAProxy gdaProxy = new GDAProxy("gdaQueryEndpoint");

            List<long> newBreakersGids;

            if (!insertedEntities.TryGetValue(DMSType.BREAKER, out newBreakersGids))
            {
                return;
            }

            List<ResourceDescription> rds = gdaProxy.GetExtentValues(DMSType.BREAKER, new List<ModelCode>() { ModelCode.PSR_MEASUREMENTS }, newBreakersGids);

            foreach (var rd in rds)
            {
                long discreteGid = rd.GetProperty(ModelCode.PSR_MEASUREMENTS).AsReferences().First();
                DMSType dmsType = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(discreteGid);

                if (dmsType != DMSType.MEASUREMENTDISCRETE)
                {
                    continue;
                }

                if (!newNeededGids.ContainsKey(dmsType))
                {
                    newNeededGids[dmsType] = new HashSet<long>();
                }

                newNeededGids[dmsType].Add(discreteGid);
            }

            base.AddAdditionalEntities(insertedEntities, newNeededGids);
        }
    }
}
