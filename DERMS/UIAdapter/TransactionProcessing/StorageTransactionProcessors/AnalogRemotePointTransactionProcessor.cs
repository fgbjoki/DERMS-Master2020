using Common.ComponentStorage;
using System.Collections.Generic;
using UIAdapter.Model;
using Common.AbstractModel;
using Common.ComponentStorage.StorageItemCreator;
using System.Threading;

namespace UIAdapter.TransactionProcessing.StorageTransactionProcessors
{
    class AnalogRemotePointTransactionProcessor : SummaryTransactionProcessor<AnalogRemotePoint>
    {
        public AnalogRemotePointTransactionProcessor(IStorage<AnalogRemotePoint> storage, Dictionary<DMSType, IStorageItemCreator> storageItemCreators, AutoResetEvent commitDone) : base(storage, storageItemCreators, commitDone)
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
