using Common.AbstractModel;
using Common.ComponentStorage;
using Common.ComponentStorage.StorageItemCreator;
using NetworkDynamicsService.Model.RemotePoints;
using NetworkDynamicsService.TransactionProcessing.StorageItemCreators;
using NetworkDynamicsService.TransactionProcessing.TransactionProcessors;
using System.Collections.Generic;

namespace NetworkDynamicsService.TransactionProcessing.Storages
{
    public class DiscreteRemotePointStorage : Storage<DiscreteRemotePoint>
    {
        public DiscreteRemotePointStorage() : base("Discrete Remote Point Storage")
        {
        }

        public override List<IStorageTransactionProcessor> GetStorageTransactionProcessors()
        {
            Dictionary<DMSType, IStorageItemCreator> itemCreators = new Dictionary<DMSType, IStorageItemCreator>()
            {
                { DMSType.MEASUREMENTDISCRETE, new DiscreteStorageItemCreator() }
            };

            return new List<IStorageTransactionProcessor>() { new DiscreteRemotePointTransactionProcessor(this, itemCreators) };
        }

        protected override IStorage<DiscreteRemotePoint> CreateNewStorage()
        {
            return new DiscreteRemotePointStorage();
        }
    }
}
