using System.Collections.Generic;
using Common.GDA;
using Common.AbstractModel;

namespace Common.ComponentStorage.StorageItemCreator
{
    public abstract class StorageItemCreator : IStorageItemCreator
    {
        protected static ModelResourcesDesc modelRescDesc = new ModelResourcesDesc();

        protected Dictionary<DMSType, List<ModelCode>> propertiesPerType;

        public abstract IdentifiedObject CreateStorageItem(ResourceDescription rd, Dictionary<DMSType, List<ResourceDescription>> affectedEntities);

        public abstract Dictionary<DMSType, List<ModelCode>> GetNeededProperties();
    }
}
