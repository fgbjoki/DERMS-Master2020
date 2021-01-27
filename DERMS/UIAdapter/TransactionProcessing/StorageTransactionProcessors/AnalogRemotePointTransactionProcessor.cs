using Common.ComponentStorage;
using System.Collections.Generic;
using UIAdapter.Model;
using Common.AbstractModel;
using Common.ComponentStorage.StorageItemCreator;

namespace UIAdapter.TransactionProcessing.StorageTransactionProcessors
{
    class AnalogRemotePointTransactionProcessor : StorageTransactionProcessor<RemotePoint>
    {
        public AnalogRemotePointTransactionProcessor(IStorage<RemotePoint> storage, Dictionary<DMSType, IStorageItemCreator> storageItemCreators) : base(storage, storageItemCreators)
        {
        }

        protected override List<DMSType> GetPrimaryTypes()
        {
            return new List<DMSType>()
            {
                DMSType.MEASUREMENTANALOG
            };
        }
    }
}
