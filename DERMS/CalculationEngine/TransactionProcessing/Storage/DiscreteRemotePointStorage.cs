using CalculationEngine.Model.Topology.Transaction;
using CalculationEngine.TransactionProcessing.StorageItemCreators;
using CalculationEngine.TransactionProcessing.StorageTransactionProcessor;
using Common.AbstractModel;
using Common.ComponentStorage;
using Common.ComponentStorage.StorageItemCreator;
using Common.PubSub;
using System.Collections.Generic;
using Common.PubSub.Subscriptions;
using System;
using CalculationEngine.PubSub.DynamicHandlers;

namespace CalculationEngine.TransactionProcessing.Storage
{
    public class DiscreteRemotePointStorage : Storage<DiscreteRemotePoint>, ISubscriber
    {
        public DiscreteRemotePointStorage() : base("Discrete remote point storage")
        {
        }

        public override List<IStorageTransactionProcessor> GetStorageTransactionProcessors()
        {
            Dictionary<DMSType, IStorageItemCreator> storageItemCreators = new Dictionary<DMSType, IStorageItemCreator>()
            {
                { DMSType.MEASUREMENTDISCRETE, new DiscreteRemotePointStorageItemCreator() },
            };

            return new List<IStorageTransactionProcessor>()
            {
                new DiscreteRemotePointTransactionProcessor(this, storageItemCreators)
            };
        }

        public override bool AddEntity(DiscreteRemotePoint item)
        {
            bool entityAdded = base.AddEntity(item);

            if (entityAdded == false)
            {
                return false;
            }

            locker.EnterWriteLock();

            if (items.ContainsKey(item.BreakerGid))
            {
                entityAdded = false;
            }
            else
            {
                items.Add(item.BreakerGid, item);
            }

            locker.ExitWriteLock();

            return entityAdded;
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
