using Common.ComponentStorage;
using System.Collections.Generic;
using UIAdapter.Model.DERs;
using Common.AbstractModel;
using Common.ComponentStorage.StorageItemCreator;

namespace UIAdapter.TransactionProcessing.StorageTransactionProcessors.DERs
{
    class EnergyStorageTransactionProcessor : StorageTransactionProcessor<EnergyStorage>
    {
        public EnergyStorageTransactionProcessor(IStorage<EnergyStorage> storage, Dictionary<DMSType, IStorageItemCreator> storageItemCreators) : base(storage, storageItemCreators)
        {
        }

        protected override List<DMSType> GetPrimaryTypes()
        {
            return new List<DMSType>(1) { DMSType.ENERGYSTORAGE };
        }
    }
}
