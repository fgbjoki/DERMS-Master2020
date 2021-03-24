using Common.AbstractModel;
using Common.ComponentStorage;
using Common.ComponentStorage.StorageItemCreator;
using Common.GDA;
using System.Collections.Generic;
using System.Threading;
using UIAdapter.Model;

namespace UIAdapter.TransactionProcessing.StorageTransactionProcessors
{
    public class DiscreteRemotePointTransactionProcessor : StorageTransactionProcessor<DiscreteRemotePoint>
    {
        public DiscreteRemotePointTransactionProcessor(IStorage<DiscreteRemotePoint> storage, Dictionary<DMSType, IStorageItemCreator> storageItemCreators) : base(storage, storageItemCreators)
        {
        }

        protected override List<DMSType> GetPrimaryTypes()
        {
            return new List<DMSType>()
            {
                DMSType.MEASUREMENTDISCRETE
            };
        }

        protected override void AddAdditionalEntities(Dictionary<DMSType, List<long>> insertedEntities, Dictionary<DMSType, HashSet<long>> newNeededGids)
        {
            GDAProxy gdaProxy = new GDAProxy("gdaQueryEndpoint");

            List<long> newDiscretePoints;

            if (!insertedEntities.TryGetValue(DMSType.MEASUREMENTDISCRETE, out newDiscretePoints))
            {
                return;
            }

            List<ResourceDescription> rds = gdaProxy.GetExtentValues(ModelCode.MEASUREMENTDISCRETE, new List<ModelCode>() { ModelCode.MEASUREMENT_PSR });

            if (rds?.Count == 0)
            {
                return;
            }

            foreach (var rd in rds)
            {
                long psrGid = rd.GetProperty(ModelCode.MEASUREMENT_PSR).AsReference();

                if (psrGid == 0)
                {
                    continue;
                }

                DMSType dmsType = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(psrGid);

                if (dmsType != DMSType.BREAKER)
                {
                    continue;
                }

                if (!newNeededGids.ContainsKey(dmsType))
                {
                    newNeededGids[dmsType] = new HashSet<long>();
                }

                newNeededGids[dmsType].Add(psrGid);
            }
        }
    }
}
