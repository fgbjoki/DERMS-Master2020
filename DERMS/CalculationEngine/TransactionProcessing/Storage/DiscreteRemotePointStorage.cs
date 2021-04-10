using CalculationEngine.Model.Topology.Transaction;
using CalculationEngine.TransactionProcessing.StorageItemCreators;
using CalculationEngine.TransactionProcessing.StorageTransactionProcessor;
using Common.AbstractModel;
using Common.ComponentStorage;
using Common.ComponentStorage.StorageItemCreator;
using System.Collections.Generic;

namespace CalculationEngine.TransactionProcessing.Storage
{
    public class DiscreteRemotePointStorage : Storage<DiscreteRemotePoint>
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

        protected override IStorage<DiscreteRemotePoint> CreateNewStorage()
        {
            return new DiscreteRemotePointStorage();
        }
    }
}
