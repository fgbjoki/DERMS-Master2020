using Common.AbstractModel;
using Common.ComponentStorage;
using Common.ComponentStorage.StorageItemCreator;
using Common.PubSub;
using System.Collections.Generic;
using UIAdapter.Model;
using UIAdapter.TransactionProcessing.StorageItemCreators;
using UIAdapter.TransactionProcessing.StorageTransactionProcessors;
using Common.PubSub.Subscriptions;
using UIAdapter.PubSub.DynamicHandlers;

namespace UIAdapter.TransactionProcessing.Storages
{
    public class AnalogRemotePointStorage : Storage<AnalogRemotePoint>, ISubscriber
    {
        public AnalogRemotePointStorage() : base("Analog Remote Point Storage")
        {
        }

        public override List<IStorageTransactionProcessor> GetStorageTransactionProcessors()
        {
            Dictionary<DMSType, IStorageItemCreator> storageItemCreators = new Dictionary<DMSType, IStorageItemCreator>()
            {
                { DMSType.MEASUREMENTANALOG, new AnalogRemotePointStorageItemCreator() }
            };

            return new List<IStorageTransactionProcessor>()
            {
                new AnalogRemotePointTransactionProcessor(this, storageItemCreators, commitDone)
            };
        }

        public IEnumerable<ISubscription> GetSubscriptions()
        {
            return new List<ISubscription>(1)
            {
                new Subscription(Topic.AnalogRemotePointChange, new AnalogRemotePointStorageDynamicHandler(this))
            };
        }

        protected override IStorage<AnalogRemotePoint> CreateNewStorage()
        {
            return new AnalogRemotePointStorage();
        }
    }
}
