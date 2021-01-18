using Common.AbstractModel;
using Common.ComponentStorage;
using Common.ComponentStorage.StorageItemCreator;
using FieldProcessor.Model;
using System.Collections.Generic;

namespace FieldProcessor.TransactionProcessing.TransactionProcessors
{
    public class DiscreteTransactionProcessor : StorageTransactionProcessor<RemotePoint>
    {
        public DiscreteTransactionProcessor(IStorage<RemotePoint> storage, Dictionary<DMSType, IStorageItemCreator> storageItemCreators) : base(storage, storageItemCreators)
        {
        }

        protected override List<DMSType> GetPrimaryTypes()
        {
            return new List<DMSType>() { DMSType.MEASUREMENTDISCRETE };
        }
    }
}
