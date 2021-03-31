using Common.ComponentStorage;
using NetworkDynamicsService.Model.RemotePoints;
using System.Collections.Generic;
using Common.AbstractModel;
using Common.ComponentStorage.StorageItemCreator;

namespace NetworkDynamicsService.TransactionProcessing.TransactionProcessors
{
    public class AnalogRemotePointTransactionProcessor : StorageTransactionProcessor<AnalogRemotePoint>
    {
        public AnalogRemotePointTransactionProcessor(IStorage<AnalogRemotePoint> storage, Dictionary<DMSType, IStorageItemCreator> storageItemCreators) : base(storage, storageItemCreators)
        {
        }

        protected override List<DMSType> GetPrimaryTypes()
        {
            return new List<DMSType>() { DMSType.MEASUREMENTANALOG };
        }
    }
}
