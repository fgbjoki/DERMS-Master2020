using Common.AbstractModel;
using Common.ComponentStorage;
using Common.ComponentStorage.StorageItemCreator;
using Common.PubSub;
using System.Collections.Generic;
using UIAdapter.Model;
using UIAdapter.TransactionProcessing.StorageItemCreators;
using UIAdapter.TransactionProcessing.StorageTransactionProcessors;
using Common.PubSub.Subscriptions;
using System;
using UIAdapter.PubSub.DynamicHandlers;

namespace UIAdapter.TransactionProcessing.Storages
{
    public class DiscreteRemotePointStorage : Storage<DiscreteRemotePoint>, ISubscriber
    {
        public DiscreteRemotePointStorage() : base("Discrete Remote Point Storage")
        {
        }

        public override List<IStorageTransactionProcessor> GetStorageTransactionProcessors()
        {
            Dictionary<DMSType, IStorageItemCreator> storageItemCreators = new Dictionary<DMSType, IStorageItemCreator>()
            {
                { DMSType.MEASUREMENTDISCRETE, new DiscreteRemotePointStorageItemCreator() }
            };

            return new List<IStorageTransactionProcessor>()
            {
                new DiscreteRemotePointTransactionProcessor(this, storageItemCreators)
            };
        }

        public IEnumerable<ISubscription> GetSubscriptions()
        {
            return new List<ISubscription>(1)
            {
                new Subscription(Topic.DiscreteRemotePointChange, new DiscreteRemotePointStorageDynamicHandler(this))
            };
        }

        protected override IStorage<DiscreteRemotePoint> CreateNewStorage()
        {
            return new DiscreteRemotePointStorage();
        }
    }
}
