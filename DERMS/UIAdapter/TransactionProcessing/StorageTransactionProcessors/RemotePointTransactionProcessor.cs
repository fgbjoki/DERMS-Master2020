using Common.ComponentStorage;
using System.Collections.Generic;
using UIAdapter.Model;
using Common.AbstractModel;
using Common.ComponentStorage.StorageItemCreator;
using Common.GDA;

namespace UIAdapter.TransactionProcessing.StorageTransactionProcessors
{
    class RemotePointTransactionProcessor : StorageTransactionProcessor<RemotePoint>
    {
        public RemotePointTransactionProcessor(IStorage<RemotePoint> storage, Dictionary<DMSType, IStorageItemCreator> storageItemCreators) : base(storage, storageItemCreators)
        {
        }

        protected override List<DMSType> GetPrimaryTypes()
        {
            return new List<DMSType>()
            {
                DMSType.MEASUREMENTANALOG, DMSType.MEASUREMENTDISCRETE
            };
        }

        protected override void AddAdditionalEntities(Dictionary<DMSType, List<long>> insertedEntities, Dictionary<DMSType, HashSet<long>> newNeededGids)
        {
            GDAProxy gdaProxy = new GDAProxy("gdaQueryEndpoint");

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
