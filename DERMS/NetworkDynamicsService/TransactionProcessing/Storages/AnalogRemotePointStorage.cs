using Common.AbstractModel;
using Common.ComponentStorage;
using Common.ComponentStorage.StorageItemCreator;
using NetworkDynamicsService.Model.RemotePoints;
using NetworkDynamicsService.TransactionProcessing.StorageItemCreators;
using NetworkDynamicsService.TransactionProcessing.TransactionProcessors;
using System.Collections.Generic;

namespace NetworkDynamicsService.TransactionProcessing.Storages
{
    public class AnalogRemotePointStorage : Storage<AnalogRemotePoint>
    {
        public AnalogRemotePointStorage() : base("Analog remote point storage")
        {
        }

        public override List<IStorageTransactionProcessor> GetStorageTransactionProcessors()
        {
            Dictionary<DMSType, IStorageItemCreator> itemCreators = new Dictionary<DMSType, IStorageItemCreator>()
            {
                { DMSType.MEASUREMENTANALOG, new AnalogStorageItemCreator() }
            };

            return new List<IStorageTransactionProcessor>() { new AnalogRemotePointTransactionProcessor(this, itemCreators) };
        }

        protected override IStorage<AnalogRemotePoint> CreateNewStorage()
        {
            return new AnalogRemotePointStorage();
        }
    }
}
