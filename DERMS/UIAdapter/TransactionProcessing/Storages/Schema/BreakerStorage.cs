using Common.AbstractModel;
using Common.ComponentStorage;
using Common.ComponentStorage.StorageItemCreator;
using System.Collections.Generic;
using UIAdapter.Model.Schema;
using UIAdapter.TransactionProcessing.StorageItemCreators.Schema;
using UIAdapter.TransactionProcessing.StorageTransactionProcessors.Schema;

namespace UIAdapter.TransactionProcessing.Storages.Schema
{
    public class BreakerStorage : Storage<Breaker>
    {
        public BreakerStorage() : base("Schema breaker state storage")
        {
        }

        public override List<IStorageTransactionProcessor> GetStorageTransactionProcessors()
        {
            Dictionary<DMSType, IStorageItemCreator> itemCreators = new Dictionary<DMSType, IStorageItemCreator>()
            { { DMSType.BREAKER, new BreakerStateStorageItemCreator() } };

            return new List<IStorageTransactionProcessor>(1) { new BreakerStateTransactionProcessor(this, itemCreators) };
        }

        protected override IStorage<Breaker> CreateNewStorage()
        {
            return new BreakerStorage();
        }

        public override bool AddEntity(Breaker item)
        {
            bool added = base.AddEntity(item);

            if (added)
            {
                items.Add(item.MeasurementDiscreteGid, item);
            }

            return added;
        }
    }
}
