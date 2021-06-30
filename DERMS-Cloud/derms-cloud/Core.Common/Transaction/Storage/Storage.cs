using Core.Common.ReliableCollectionProxy;
using Core.Common.Transaction.StorageTransactionProcessor;
using Microsoft.ServiceFabric.Data;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Core.Common.Transaction.Storage
{
    public abstract class Storage<T> : ITransactionStorage, IStorage<T>
        where T : IdentifiedObject
    {
        protected string dictionaryName;

        protected string storageName;

        protected AutoResetEvent commitDone;

        protected IReliableStateManager stateManager;

        public Storage(IReliableStateManager stateManager, string storageName, string dictionaryName = "items")
        {
            this.storageName = storageName;
            this.stateManager = stateManager;
            this.dictionaryName = dictionaryName + storageName;

            commitDone = new AutoResetEvent(false);

            ReliableDictionaryProxy.CreateDictionary<T, long>(stateManager, dictionaryName);
        }

        public virtual bool AddEntity(T item)
        {
            if (item == null)
            {
                Log($"[{storageName}] Cannot add null to storage!");
                return false;
            }

            if (EntityExists(item.GlobalId))
            {
                Log($"[{storageName}] already contains entity with gid: 0x{item.GlobalId:X16}");
                return false;
            }

            ReliableDictionaryProxy.AddOrUpdateEntity(stateManager, item, item.GlobalId, dictionaryName);

            return true;
        }

        public List<T> GetAllEntities()
        {
            return ReliableDictionaryProxy.GetAllEntities<T, long>(stateManager, dictionaryName);
        }

        public virtual T GetEntity(long globalId)
        {
            return ReliableDictionaryProxy.GetEntity<T, long>(stateManager, globalId, dictionaryName);
        }

        public void UpdateEntityProperty(long entityGid, Predicate<T> predicate)
        {
            T entity = GetEntity(entityGid);

            if (entity == null)
            {
                return;
            }

            predicate.Invoke(entity);
            ReliableDictionaryProxy.AddOrUpdateEntity(stateManager, entity, entity.GlobalId, dictionaryName);
        }

        public virtual bool EntityExists(long globalId)
        {
            return ReliableDictionaryProxy.EntityExists<T, long>(stateManager, globalId, dictionaryName);
        }

        public AutoResetEvent Commited { get { return commitDone; } }

        public abstract List<IStorageTransactionProcessor> GetStorageTransactionProcessors();

        public virtual bool ValidateEntity(T entity)
        {
            return entity != null;
        }

        public object Clone()
        {
            IStorage<T> clonedStorage = CreateNewStorage(dictionaryName + storageName + "clonedItems");

            foreach (var item in ReliableDictionaryProxy.Iterate<T, long>(stateManager, dictionaryName))
            {
                clonedStorage.AddEntity(item);
            }

            return clonedStorage;
        }

        public void ShallowCopyEntities(IStorage<T> storage)
        {
            Storage<T> copyStorage = storage as Storage<T>;

            ReliableDictionaryProxy.CopyDictionary<T, long>(stateManager, dictionaryName, copyStorage.dictionaryName);
        }


        protected abstract IStorage<T> CreateNewStorage(string dictionaryName);

        protected abstract void Log(string text);
    }
}
