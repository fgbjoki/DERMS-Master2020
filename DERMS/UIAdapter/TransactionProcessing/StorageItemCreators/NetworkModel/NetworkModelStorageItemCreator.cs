using Common.ComponentStorage.StorageItemCreator;
using System.Collections.Generic;
using Common.AbstractModel;
using Common.ComponentStorage;
using Common.GDA;
using UIAdapter.Model.NetworkModel;

namespace UIAdapter.TransactionProcessing.StorageItemCreators.NetworkModel
{
    public class NetworkModelStorageItemCreator : StorageItemCreator
    {
        public override IdentifiedObject CreateStorageItem(ResourceDescription rd, Dictionary<DMSType, List<ResourceDescription>> affectedEntities)
        {
            NetworkModelItem identifiedObject = new NetworkModelItem(rd.Id)
            {
                Name = rd.GetProperty(ModelCode.IDOBJ_NAME).AsString()
            };

            return identifiedObject;
        }

        public override Dictionary<DMSType, List<ModelCode>> GetNeededProperties()
        {
            List<DMSType> dmsTypes = ModelResourcesDesc.GetLeavesForCoreEntities(ModelCode.CONDUCTINGEQ);
            dmsTypes.Remove(DMSType.ACLINESEG);

            List<ModelCode> neededModelCodes = new List<ModelCode>()
            {
                ModelCode.IDOBJ_NAME
            };

            Dictionary<DMSType, List<ModelCode>> neededProperties = new Dictionary<DMSType, List<ModelCode>>(dmsTypes.Count);

            foreach (var dmsType in dmsTypes)
            {
                neededProperties.Add(dmsType, neededModelCodes);
            }

            return neededProperties;
        }
    }
}
