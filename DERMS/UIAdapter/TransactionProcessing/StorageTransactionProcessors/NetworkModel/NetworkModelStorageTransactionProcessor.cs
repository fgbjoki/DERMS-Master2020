using Common.ComponentStorage;
using System.Collections.Generic;
using Common.AbstractModel;
using Common.ComponentStorage.StorageItemCreator;
using UIAdapter.Model.NetworkModel;

namespace UIAdapter.TransactionProcessing.StorageTransactionProcessors.NetworkModel
{
    public class NetworkModelStorageTransactionProcessor : StorageTransactionProcessor<NetworkModelItem>
    {
        public NetworkModelStorageTransactionProcessor(IStorage<NetworkModelItem> storage, Dictionary<DMSType, IStorageItemCreator> storageItemCreators) : base(storage, storageItemCreators)
        {
        }

        protected override List<DMSType> GetPrimaryTypes()
        {
            List<DMSType> dmsTypes = ModelResourcesDesc.GetLeavesForCoreEntities(ModelCode.CONDUCTINGEQ);
            dmsTypes.Remove(DMSType.ACLINESEG);

            return dmsTypes;
        }
    }
}
