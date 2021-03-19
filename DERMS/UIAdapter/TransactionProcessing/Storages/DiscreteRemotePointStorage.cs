using Common.AbstractModel;
using Common.ComponentStorage;
using Common.ComponentStorage.StorageItemCreator;
using System.Collections.Generic;
using UIAdapter.DynamicHandlers;
using UIAdapter.Model;
using UIAdapter.TransactionProcessing.StorageItemCreators;
using UIAdapter.TransactionProcessing.StorageTransactionProcessors;

namespace UIAdapter.TransactionProcessing.Storages
{
    public class DiscreteRemotePointStorage : Storage<DiscreteRemotePoint>, INServiceBusStorage
    {
        public DiscreteRemotePointStorage() : base("Discrete Remote Point Storage")
        {
        }

        public List<object> GetHandlers()
        {
            return new List<object>() { new DiscreteRemotePointChangedHandler(this) };
        }

        public override List<IStorageTransactionProcessor> GetStorageTransactionProcessors()
        {
            Dictionary<DMSType, IStorageItemCreator> storageItemCreators = new Dictionary<DMSType, IStorageItemCreator>()
            {
                { DMSType.MEASUREMENTDISCRETE, new DiscreteRemotePointStorageItemCreator() }
            };

            return new List<IStorageTransactionProcessor>()
            {
                new DiscreteRemotePointTransactionProcessor(this, storageItemCreators, commitDone)
            };
        }

        public override bool ValidateEntity(DiscreteRemotePoint entity)
        {
            return true;
        }
    }
}
