using Common.AbstractModel;
using Common.GDA;
using System.Collections.Generic;

namespace Common.ComponentStorage.StorageItemCreator
{
    public interface IStorageItemCreator
    {
        Dictionary<DMSType, List<ModelCode>> GetNeededProperties();
        IdentifiedObject CreateStorageItem(ResourceDescription rd, Dictionary<DMSType, List<ResourceDescription>> affectedEntities);
    }
}
