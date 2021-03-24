using Common.ComponentStorage;
using System.Collections.Generic;
using UIAdapter.Model;
using Common.AbstractModel;
using Common.ComponentStorage.StorageItemCreator;
using System.Threading;

namespace UIAdapter.TransactionProcessing.StorageTransactionProcessors
{
    class AnalogRemotePointTransactionProcessor : StorageTransactionProcessor<AnalogRemotePoint>
    {
        public AnalogRemotePointTransactionProcessor(IStorage<AnalogRemotePoint> storage, Dictionary<DMSType, IStorageItemCreator> storageItemCreators) : base(storage, storageItemCreators)
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
