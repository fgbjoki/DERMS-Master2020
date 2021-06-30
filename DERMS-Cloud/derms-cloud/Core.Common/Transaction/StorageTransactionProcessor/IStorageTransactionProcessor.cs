using Core.Common.AbstractModel;
using Core.Common.GDA;
using System.Collections.Generic;

namespace Core.Common.Transaction.StorageTransactionProcessor
{
    public interface IStorageTransactionProcessor
    {
        bool Prepare(Dictionary<DMSType, List<ResourceDescription>> affectedEntities);

        bool Commit();

        bool Rollback();

        bool ApplyChanges(Dictionary<DMSType, List<long>> insertedEntities, Dictionary<DMSType, HashSet<long>> newNeededGids);

        Dictionary<DMSType, List<ModelCode>> GetNeededProperties();
    }
}
