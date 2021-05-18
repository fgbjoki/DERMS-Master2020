using Common.AbstractModel;
using Common.ComponentStorage;
using Common.ComponentStorage.StorageItemCreator;
using System.Collections.Generic;
using UIAdapter.Model.NetworkModel;
using UIAdapter.TransactionProcessing.StorageItemCreators.NetworkModel;
using UIAdapter.TransactionProcessing.StorageTransactionProcessors.NetworkModel;

namespace UIAdapter.TransactionProcessing.Storages.NetworkModel
{
    public class NetworkModelStorage : Storage<NetworkModelItem>
    {
        public NetworkModelStorage() : base("Network Model storage")
        {
        }

        public override List<IStorageTransactionProcessor> GetStorageTransactionProcessors()
        {
            List<DMSType> dmsTypes = ModelResourcesDesc.GetLeavesForCoreEntities(ModelCode.CONDUCTINGEQ);
            dmsTypes.Remove(DMSType.ACLINESEG);

            Dictionary<DMSType, IStorageItemCreator> creators = new Dictionary<DMSType, IStorageItemCreator>();

            var storageItemCreator = new NetworkModelStorageItemCreator();

            foreach (var dmsType in dmsTypes)
            {
                creators.Add(dmsType, storageItemCreator);
            }

            return new List<IStorageTransactionProcessor>() { new NetworkModelStorageTransactionProcessor(this, creators) };
        }

        protected override IStorage<NetworkModelItem> CreateNewStorage()
        {
            return new NetworkModelStorage();
        }
    }
}
