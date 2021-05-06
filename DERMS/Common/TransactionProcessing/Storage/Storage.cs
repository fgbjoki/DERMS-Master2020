using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Common.ComponentStorage
{
    public abstract class Storage<T> : ITransactionStorage, IStorage<T>
        where T : IdentifiedObject
    {
        protected Dictionary<long, T> items;

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
                Logger.Logger.Instance.Log($"[{storageName}] Cannot add null to storage!");
                return false;
            }

            if (EntityExists(item.GlobalId))
            {
                Logger.Logger.Instance.Log($"[{storageName}] already contains entity with gid: 0x{item.GlobalId:X16}");
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

        public virtual T GetEntity(long globalId)
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

        public void UpdateEntityProperty(long entityGid, Predicate<T> predicate)
        {
            T entity = GetEntity(entityGid);

            if (entity == null)
            {
                return;
            }

            locker.EnterWriteLock();
            predicate.Invoke(entity);
            locker.ExitWriteLock();
        }

        public virtual bool EntityExists(long globalId)
        {       
            locker.EnterReadLock();
            bool entityExists = items.ContainsKey(globalId);
            locker.ExitReadLock();

            return entityExists;
        }

        public AutoResetEvent Commited { get { return commitDone; } }

        public abstract List<IStorageTransactionProcessor> GetStorageTransactionProcessors();

        public virtual bool ValidateEntity(T entity)
        {
            return entity != null;
        }

        public object Clone()
        {
            IStorage<T> clonedStorage = CreateNewStorage();

            locker.EnterReadLock();

            foreach (var item in items)
            {
                clonedStorage.AddEntity(item.Value);
            }

            locker.ExitReadLock();

            return clonedStorage;
        }

        protected abstract IStorage<T> CreateNewStorage();

        public void ShallowCopyEntities(IStorage<T> storage)
        {
            Storage<T> copyStorage = storage as Storage<T>;

            copyStorage.locker.EnterReadLock();
            locker.EnterWriteLock();

            items = copyStorage.items;

            copyStorage.locker.ExitReadLock();
            locker.ExitWriteLock();
        }
    }
}
