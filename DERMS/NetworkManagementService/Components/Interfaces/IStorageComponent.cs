using Common.AbstractModel;
using NetworkManagementService.DataModel.Core;
using System.Collections.Generic;

namespace NetworkManagementService.Components
{
    public interface IStorageComponent
    {
        IdentifiedObject GetEntity(long globalId, ModelAccessScope accesScope = ModelAccessScope.CurrentModel);

        List<long> GetEntitiesIdByDMSType(DMSType dmsType, ModelAccessScope accessScope = ModelAccessScope.CurrentModel);
    }
}
