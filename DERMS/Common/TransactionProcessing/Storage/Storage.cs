using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Common.ComponentStorage
{
    public abstract class Storage<T> : ITransactionStorage, IStorage<T>
        where T : IdentifiedObject
    {
        private Dictionary<long, T> items;

        protected string storageName;

        protected ReaderWriterLockSlim locker;

        protected AutoResetEvent commitDone;

        public Storage(string storageName)
        {
            commitDone = new AutoResetEvent(false);

            this.storageName = storageName;

            locker = new ReaderWriterLockSlim();

            items = new Dictionary<long, T>();
        }

        public virtual bool AddEntity(T item)
        {
            if (item == null)
            {
                Common.Logger.Logger.Instance.Log($"[{storageName}] Cannot add null to storage!");
                return false;
            }

            if (EntityExists(item.GlobalId))
            {
                Common.Logger.Logger.Instance.Log($"[{storageName}] already contains entity with gid: 0x{item.GlobalId:8X}");
                return false;
            }

            locker.EnterWriteLock();
            items.Add(item.GlobalId, item);
            locker.ExitWriteLock();

            return true;
        }

        public List<T> GetAllEntities()
        {
            locker.EnterReadLock();
            List<T> allEntities = items.Values.ToList();
            locker.ExitReadLock();

            return allEntities;
        }

        public T GetEntity(long globalId)
        {
            if (!EntityExists(globalId))
            {
                return null;
            }

            locker.EnterReadLock();

            T entity =  items[globalId];

            locker.ExitReadLock();

            return entity;
        }

        public bool EntityExists(long globalId)
        {       
            locker.EnterReadLock();
            bool entityExists = items.ContainsKey(globalId);
            locker.ExitReadLock();

            return entityExists;
        }

        public AutoResetEvent Commited { get { return commitDone; } }

        public abstract List<IStorageTransactionProcessor> GetStorageTransactionProcessors();

        public abstract bool ValidateEntity(T entity);
    }
}
