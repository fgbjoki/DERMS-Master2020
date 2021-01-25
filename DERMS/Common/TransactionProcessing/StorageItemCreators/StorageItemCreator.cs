using System.Collections.Generic;
using Common.GDA;
using Common.AbstractModel;

namespace Common.ComponentStorage.StorageItemCreator
{
    public abstract class StorageItemCreator : IStorageItemCreator
    {
        protected static ModelResourcesDesc modelRescDesc = new ModelResourcesDesc();

        protected Dictionary<ModelCode, List<ModelCode>> propertiesPerType;

        protected StorageItemCreator(Dictionary<ModelCode, List<ModelCode>> propertiesPerType)
        {
            this.propertiesPerType = propertiesPerType;
        }

        public abstract IdentifiedObject CreateStorageItem(ResourceDescription rd, Dictionary<DMSType, List<ResourceDescription>> affectedEntities);

        public Dictionary<ModelCode, List<ModelCode>> GetNeededProperties()
        {
            return propertiesPerType;
        }
    }
}
