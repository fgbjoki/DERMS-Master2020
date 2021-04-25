using Common.AbstractModel;
using Common.ComponentStorage;
using Common.ComponentStorage.StorageItemCreator;
using System.Collections.Generic;
using UIAdapter.Model.DERs;
using UIAdapter.TransactionProcessing.StorageItemCreators.DERs;
using UIAdapter.TransactionProcessing.StorageTransactionProcessors.DERs;

namespace UIAdapter.TransactionProcessing.Storages.DERs
{
    public class EnergyStorageStorage : Storage<EnergyStorage>
    {
        public EnergyStorageStorage() : base("DER: Energy storage")
        {
        }

        public override List<IStorageTransactionProcessor> GetStorageTransactionProcessors()
        {
            Dictionary<DMSType, IStorageItemCreator> creators = new Dictionary<DMSType, IStorageItemCreator>()
            {
                { DMSType.ENERGYSTORAGE, new EnergyStorageStorageItemCreator() }
            };

            return new List<IStorageTransactionProcessor>(1) { new EnergyStorageTransactionProcessor(this, creators) };
        }

        protected override IStorage<EnergyStorage> CreateNewStorage()
        {
            return new EnergyStorageStorage();
        }
    }
}
