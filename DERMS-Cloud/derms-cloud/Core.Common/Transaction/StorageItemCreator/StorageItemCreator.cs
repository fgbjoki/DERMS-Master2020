using System.Collections.Generic;
using Core.Common.GDA;
using Core.Common.AbstractModel;
using Core.Common.Transaction.Models;

namespace Core.Common.Transaction.StorageItemCreator
{
    public abstract class StorageItemCreator : IStorageItemCreator
    {
        protected static ModelResourcesDesc modelRescDesc = new ModelResourcesDesc();

        protected Dictionary<DMSType, List<ModelCode>> propertiesPerType;

        public abstract IdentifiedObject CreateStorageItem(ResourceDescription rd, Dictionary<DMSType, List<ResourceDescription>> affectedEntities);

        public abstract Dictionary<DMSType, List<ModelCode>> GetNeededProperties();
    }
}
