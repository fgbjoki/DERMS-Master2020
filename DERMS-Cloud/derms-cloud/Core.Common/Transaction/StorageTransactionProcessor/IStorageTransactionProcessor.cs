using Core.Common.AbstractModel;
using Core.Common.GDA;
using Microsoft.ServiceFabric.Data;
using System.Collections.Generic;

namespace Core.Common.Transaction.StorageTransactionProcessor
{
    public interface IStorageTransactionProcessor
    {
        bool Prepare(IReliableStateManager stateManager, Dictionary<DMSType, List<ResourceDescription>> affectedEntities);

        bool Commit(IReliableStateManager stateManager);

        bool Rollback(IReliableStateManager stateManager);

        bool ApplyChanges(Dictionary<DMSType, List<long>> insertedEntities, IReliableStateManager stateManager);

        Dictionary<DMSType, List<ModelCode>> GetNeededProperties();
    }
}
