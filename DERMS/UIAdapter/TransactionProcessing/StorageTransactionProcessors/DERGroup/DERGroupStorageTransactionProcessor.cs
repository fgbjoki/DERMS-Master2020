using Common.ComponentStorage;
using System.Collections.Generic;
using Common.AbstractModel;
using Common.ComponentStorage.StorageItemCreator;

namespace UIAdapter.TransactionProcessing.StorageTransactionProcessors.DERGroup
{
    public class DERGroupStorageTransactionProcessor : StorageTransactionProcessor<Model.DERGroup.DERGroup>
    {
        public DERGroupStorageTransactionProcessor(IStorage<Model.DERGroup.DERGroup> storage, Dictionary<DMSType, IStorageItemCreator> storageItemCreators) : base(storage, storageItemCreators)
        {
        }

        protected override List<DMSType> GetPrimaryTypes()
        {
            return new List<DMSType>(1) { DMSType.ENERGYSTORAGE };
        }
    }
}
