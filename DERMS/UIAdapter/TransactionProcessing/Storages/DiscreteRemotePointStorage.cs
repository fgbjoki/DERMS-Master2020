using Common.AbstractModel;
using Common.ComponentStorage;
using Common.ComponentStorage.StorageItemCreator;
using System.Collections.Generic;
using UIAdapter.Model;
using UIAdapter.TransactionProcessing.StorageItemCreators;
using UIAdapter.TransactionProcessing.StorageTransactionProcessors;

namespace UIAdapter.TransactionProcessing.Storages
{
    public class DiscreteRemotePointStorage : Storage<DiscreteRemotePoint>
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
                new DiscreteRemotePointTransactionProcessor(this, storageItemCreators, commitDone)
            };
        }

        public override bool ValidateEntity(DiscreteRemotePoint entity)
        {
            return true;
        }
    }
}
