using Core.Common.AbstractModel;
using Core.Common.GDA;
using Core.Common.PropertyWrappers.EnumWrappers;
using Core.Common.ReliableCollectionProxy;
using Core.Common.Transaction.Models;
using Core.Common.Transaction.Storage;
using Core.Common.Transaction.StorageItemCreator;
using Microsoft.ServiceFabric.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Core.Common.Transaction.StorageTransactionProcessor
{
    public abstract class StorageTransactionProcessor<T> : IStorageTransactionProcessor
        where T : IdentifiedObject
    {
        protected readonly string newNeededGidsDictionary;
        protected readonly string preparedObjectsDictionary;

        protected static readonly ModelResourcesDesc modelRescDesc = new ModelResourcesDesc();

        private Dictionary<DMSType, IStorageItemCreator> storageItemCreators;

        private AutoResetEvent commitDone;

        protected IStorage<T> storage;
        protected IStorage<T> temporaryTransactionStorage;

        protected StorageTransactionProcessor(IStorage<T> storage, Dictionary<DMSType, IStorageItemCreator> storageItemCreators)
        {
            this.storage = storage;
            this.storageItemCreators = storageItemCreators;

            newNeededGidsDictionary = "newNeededGids";
            preparedObjectsDictionary = GetType().Name + "preparedObjects";

            temporaryTransactionStorage = storage.CreateTransactionCopy();
        }

        public virtual bool Commit(IReliableStateManager stateManager)
        {
            bool commited = true;

            storage.ShallowCopyEntities(temporaryTransactionStorage);

            //commitDone.Set();

            DisposeTransactionResources(stateManager);

            return commited;
        }

        public bool Prepare(IReliableStateManager stateManager, Dictionary<DMSType, List<ResourceDescription>> affectedEntities)
        {
            temporaryTransactionStorage.ShallowCopyEntities(storage);

            if (!ProcessPrimaryEntities(stateManager, affectedEntities))
            {
                Log($"[{this.GetType().Name}] Failed on Prepare!");
                return false;
            }
            
            if (!AdditionalProcessing(affectedEntities))
            {
                Log($"[{this.GetType().Name}] Failed on Prepare!");
                return false;
            }

            return true;
        }

        public virtual bool Rollback(IReliableStateManager stateManager)
        {
            DisposeTransactionResources(stateManager);

            return true;
        }

        public bool ApplyChanges(Dictionary<DMSType, List<long>> insertedEntities, IReliableStateManager stateManager)
        {
            var primaryTypes = GetPrimaryTypes();
            foreach (var newEntitiesPerType in insertedEntities)
            {
                if (!primaryTypes.Contains(newEntitiesPerType.Key))
                {
                    continue;
                }

                foreach (var newGid in newEntitiesPerType.Value)
                {
                    if (storage.EntityExists(newGid))
                    {
                        Log($"[{this.GetType().Name}] Failed on apply changes! Entity with given GID already exists.");
                        return false;
                    }
                }

                int key = (int)newEntitiesPerType.Key;

                if (!ReliableDictionaryProxy.EntityExists<HashSet<long>, int>(stateManager, key, newNeededGidsDictionary))
                {
                    ReliableDictionaryProxy.AddOrUpdateEntity(stateManager, new HashSet<long>(newEntitiesPerType.Value), key, newNeededGidsDictionary);
                }

            }

            AddAdditionalEntities(insertedEntities, stateManager);

            return true;
        }

        protected abstract List<DMSType> GetPrimaryTypes();


        protected virtual void AddAdditionalEntities(Dictionary<DMSType, List<long>> insertedEntities, IReliableStateManager stateManager)
        {
        }

        protected virtual bool AdditionalProcessing(Dictionary<DMSType, List<ResourceDescription>> affectedEntities)
        {
            return true;
        }

        protected virtual void DisposeTransactionResources(IReliableStateManager stateManager)
        {
            
        }

        private bool ProcessPrimaryEntities(IReliableStateManager stateManager, Dictionary<DMSType, List<ResourceDescription>> affectedEntities)
        {
            ReliableDictionaryProxy.CreateDictionary<T, long>(stateManager, preparedObjectsDictionary);

            foreach (var oneTypeEntities in GetProcessingOrder(affectedEntities))
            {
                IStorageItemCreator storageItemCreator;
                if (!storageItemCreators.TryGetValue(oneTypeEntities.Key, out storageItemCreator))
                {
                    continue;
                }

                foreach (var newEntityRd in oneTypeEntities.Value)
                {
                    IdentifiedObject newStorageItem = storageItemCreator.CreateStorageItem(newEntityRd, affectedEntities);
                    T newItem = newStorageItem as T;

                    if (!temporaryTransactionStorage.ValidateEntity(newItem))
                    {
                        return false;
                    }

                    temporaryTransactionStorage.AddEntity(newItem);
                    ReliableDictionaryProxy.AddOrUpdateEntity(stateManager, newItem, newItem.GlobalId, preparedObjectsDictionary);
                }
            }

            return true;
        }

        public Dictionary<DMSType, List<ModelCode>> GetNeededProperties()
        {
            Dictionary<DMSType, HashSet<ModelCode>> neededPropeties = new Dictionary<DMSType, HashSet<ModelCode>>();
            foreach (var storageItemCreator in storageItemCreators.Values)
            {
                Dictionary<DMSType, List<ModelCode>> propertyPerCreator = storageItemCreator.GetNeededProperties();

                foreach (var properties in propertyPerCreator)
                {
                    if (!neededPropeties.ContainsKey(properties.Key))
                    {
                        neededPropeties[properties.Key] = new HashSet<ModelCode>(properties.Value);
                    }

                    HashSet<ModelCode> hashSet = neededPropeties[properties.Key];
                    foreach (var property in properties.Value)
                    {
                        hashSet.Add(property);
                    }

                }
            }

            Dictionary<DMSType, List<ModelCode>> returnValue = new Dictionary<DMSType, List<ModelCode>>();

            foreach (var property in neededPropeties)
            {
                returnValue[property.Key] = property.Value.ToList();
            }

            return returnValue;
        }

        protected virtual IEnumerable<KeyValuePair<DMSType, List<ResourceDescription>>> GetProcessingOrder(Dictionary<DMSType, List<ResourceDescription>> affectedEntities)
        {
            return affectedEntities;
        }

        protected abstract void Log(string text);
    }
}
