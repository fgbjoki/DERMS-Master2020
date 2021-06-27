using Core.Common.GDA;
using System.Collections.Generic;

namespace NetworkManagementService.Components
{
    interface IInsertionComponent
    {
        void InsertEntity(ResourceDescription rd);

        Dictionary<short, int> GetCounters(ModelAccessScope accesScope = ModelAccessScope.ApplyDelta);

        void ApplyDeltaPreparation();

        void ApplyDeltaFailed();
    }
}
