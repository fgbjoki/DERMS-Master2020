using Core.Common.ReliableCollectionProxy;
using Core.Common.Transaction.StorageTransactionProcessor;
using Microsoft.ServiceFabric.Data;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading;

namespace Core.Common.Transaction.Storage
{
    [DataContract]
    public abstract class Storage<T> : ITransactionStorage, IStorage<T>
        where T : IdentifiedObject
    {
        protected string dictionaryName;

        protected string storageName;

        protected IReliableStateManager stateManager;

        protected AutoResetEvent commitDone;

        protected Storage(IReliableStateManager stateManager, string storageName, string dictionaryName = "items")
        {
            this.storageName = storageName;
            this.stateManager = stateManager;
            this.dictionaryName = dictionaryName + storageName;

            commitDone = new AutoResetEvent(false);

            ReliableDictionaryProxy.CreateDictionary<T, long>(stateManager, dictionaryName);
        }

        [DataMember]
        public string DictionaryName { get => dictionaryName; set => dictionaryName = value; }
        [DataMember]
        public string StorageName { get => storageName; set => storageName = value; }
        [DataMember]
        public IReliableStateManager StateManager { get => stateManager; set => stateManager = value; }

        public AutoResetEvent Commited { get { return commitDone; } }

        public virtual bool AddEntity(T entity)
        {
            if (entity == null)
            {
                Log($"[{storageName}] Cannot add null to storage!");
                return false;
            }

            if (EntityExists(entity.GlobalId))
            {
                Log($"[{storageName}] already contains entity with gid: 0x{entity.GlobalId:X16}");
                return false;
            }

            ReliableDictionaryProxy.AddOrUpdateEntity(stateManager, entity, entity.GlobalId, dictionaryName);

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

        public abstract List<IStorageTransactionProcessor> GetStorageTransactionProcessors();

        public virtual bool ValidateEntity(T entity)
        {
            return entity != null;
        }

        public void ShallowCopyEntities(IStorage<T> storage)
        {
            Storage<T> copyStorage = storage as Storage<T>;

            ReliableDictionaryProxy.CopyDictionary<T, long>(stateManager, dictionaryName, copyStorage.dictionaryName);
        }

        protected abstract IStorage<T> CreateNewStorage(string name, string dictionaryName);

        protected abstract void Log(string text);

        public IStorage<T> CreateTransactionCopy()
        {
            return CreateNewStorage("Temporary" + storageName, dictionaryName + storageName + "clonedItems");
        }
    }
}
