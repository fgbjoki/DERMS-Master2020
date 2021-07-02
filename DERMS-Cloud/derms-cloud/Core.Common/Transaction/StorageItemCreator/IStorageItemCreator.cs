using Core.Common.AbstractModel;
using Core.Common.GDA;
using Core.Common.Transaction.Storage;
using System.Collections.Generic;

namespace Core.Common.Transaction.StorageItemCreator
{
    public interface IStorageItemCreator
    {
        Dictionary<DMSType, List<ModelCode>> GetNeededProperties();
        IdentifiedObject CreateStorageItem(ResourceDescription rd, Dictionary<DMSType, List<ResourceDescription>> affectedEntities);
    }
}
