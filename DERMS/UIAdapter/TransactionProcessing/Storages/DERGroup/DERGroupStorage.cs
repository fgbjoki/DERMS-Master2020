using System;
using System.Collections.Generic;
using Common.ComponentStorage;
using Common.PubSub;
using Common.PubSub.Subscriptions;
using Common.AbstractModel;
using Common.ComponentStorage.StorageItemCreator;
using UIAdapter.TransactionProcessing.StorageItemCreators.DERGroup;
using UIAdapter.TransactionProcessing.StorageTransactionProcessors.DERGroup;

namespace UIAdapter.TransactionProcessing.Storages.DERGroup
{
    public class DERGroupStorage : Storage<Model.DERGroup.DERGroup>, ISubscriber
    {
        public DERGroupStorage() : base("DERGroup storage")
        {
        }

        public override List<IStorageTransactionProcessor> GetStorageTransactionProcessors()
        {
            Dictionary<DMSType, IStorageItemCreator> itemCreators = new Dictionary<DMSType, IStorageItemCreator>()
            {
                { DMSType.ENERGYSTORAGE, new DERGroupStorageItemCreator() }
            };

            return new List<IStorageTransactionProcessor>(1) { new DERGroupStorageTransactionProcessor(this, itemCreators) };
        }

        public IEnumerable<ISubscription> GetSubscriptions()
        {
            throw new NotImplementedException();
        }

        protected override IStorage<Model.DERGroup.DERGroup> CreateNewStorage()
        {
            return new DERGroupStorage();
        }
    }
}
