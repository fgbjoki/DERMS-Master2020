using Common.Logger;
using System.Collections.Generic;
using System.Linq;

namespace Common.ComponentStorage
{
    public abstract class Storage<T> : ITransactionStorage, IStorage<T>
        where T : IdentifiedObject
    {
        private Dictionary<long, T> items;

        protected string storageName;

        public Storage(string storageName)
        {
            this.storageName = storageName;

            items = new Dictionary<long, T>();
        }

        public virtual bool AddEntity(T item)
        {
            if (item == null)
            {
                DERMSLogger.Instance.Log($"[{storageName}] Cannot add null to storage!");
                return false;
            }

            if (EntityExists(item.GlobalId))
            {
                DERMSLogger.Instance.Log($"[{storageName}] already contains entity with gid: 0x{item.GlobalId:8X}");
                return false;
            }

            items.Add(item.GlobalId, item);

            return true;
        }

        public List<T> GetAllEntities()
        {
            return items.Values.ToList();
        }

        public T GetEntity(long globalId)
        {
            if (EntityExists(globalId))
            {
                return items[globalId];
            }

            return null;
        }

        public bool EntityExists(long globalId)
        {
            return items.ContainsKey(globalId);
        }

        public abstract List<IStorageTransactionProcessor> GetStorageTransactionProcessors();

        public abstract bool ValidateEntity(T entity);
    }
}
