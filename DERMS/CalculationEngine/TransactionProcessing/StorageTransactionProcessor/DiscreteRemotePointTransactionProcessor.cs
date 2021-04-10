using CalculationEngine.Model.Topology.Transaction;
using Common.ComponentStorage;
using System.Collections.Generic;
using Common.AbstractModel;
using Common.ComponentStorage.StorageItemCreator;

namespace CalculationEngine.TransactionProcessing.StorageTransactionProcessor
{
    public class DiscreteRemotePointTransactionProcessor : StorageTransactionProcessor<DiscreteRemotePoint>
    {
        public DiscreteRemotePointTransactionProcessor(IStorage<DiscreteRemotePoint> storage, Dictionary<DMSType, IStorageItemCreator> storageItemCreators) : base(storage, storageItemCreators)
        {
        }

        protected override List<DMSType> GetPrimaryTypes()
        {
            return new List<DMSType>() { DMSType.MEASUREMENTDISCRETE };
        }
    }
}
