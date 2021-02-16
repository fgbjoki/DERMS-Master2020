using Common.AbstractModel;
using Common.GDA;
using System.Collections.Generic;

namespace Common.ComponentStorage
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
