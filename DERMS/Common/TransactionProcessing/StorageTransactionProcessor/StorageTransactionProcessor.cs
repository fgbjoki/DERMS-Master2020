using Common.AbstractModel;
using Common.ComponentStorage.StorageItemCreator;
using Common.GDA;
using Common.Logger;
using System.Collections.Generic;
using System.Linq;

namespace Common.ComponentStorage
{
    public abstract class StorageTransactionProcessor<T> : IStorageTransactionProcessor
        where T : IdentifiedObject
    {
        private Dictionary<DMSType, IStorageItemCreator> storageItemCreators;

        private List<DMSType> primaryTypes;

        protected Dictionary<long, T> preparedObjects;

        protected  IStorage<T> storage;

        protected StorageTransactionProcessor(IStorage<T> storage, Dictionary<DMSType, IStorageItemCreator> storageItemCreators)
        {
            this.storage = storage;
            this.storageItemCreators = storageItemCreators;

            primaryTypes = GetPrimaryTypes();
        }

        public virtual bool Commit()
        {
            bool commited = true;
            foreach (var preparedObject in preparedObjects.Values)
            {
                if (!storage.AddEntity(preparedObject))
                {
                    DERMSLogger.Instance.Log($"[{this.GetType().Name}] Failed on Commit!");
                    commited = false;
                    break;
                }
            }

            DisposeTransactionResources();

            return commited;
        }

        public bool Prepare(Dictionary<DMSType, List<ResourceDescription>> affectedEntities)
        {
            if (!ProcessPrimaryEntities(affectedEntities))
            {
                return false;
            }
            
            if (!AdditionalProcessing(affectedEntities))
            {
                return false;
            }

            return true;
        }

        public virtual bool Rollback()
        {
            DisposeTransactionResources();

            return true;
        }

        public bool ApplyChanges(Dictionary<DMSType, List<long>> insertedEntities, Dictionary<DMSType, HashSet<long>> neededGids)
        {
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
                        DERMSLogger.Instance.Log($"[{this.GetType().Name}] Failed on apply changes! Entity with given GID already exists.");
                        return false;
                    }
                }

                if (!neededGids.ContainsKey(newEntitiesPerType.Key))
                {
                    neededGids[newEntitiesPerType.Key] = new HashSet<long>(newEntitiesPerType.Value);
                }

            }

            AddAdditionalEntities(insertedEntities, neededGids);

            return true;
        }

        protected abstract List<DMSType> GetPrimaryTypes();


        protected virtual void AddAdditionalEntities(Dictionary<DMSType, List<long>> insertedEntities, Dictionary<DMSType, HashSet<long>> newNeededGids)
        {
            return;
        }

        protected virtual bool AdditionalProcessing(Dictionary<DMSType, List<ResourceDescription>> affectedEntities)
        {
            return true;
        }

        protected virtual void DisposeTransactionResources()
        {
            preparedObjects.Clear();
        }

        private bool ProcessPrimaryEntities(Dictionary<DMSType, List<ResourceDescription>> affectedEntities)
        {
            preparedObjects = new Dictionary<long, T>();

            foreach (var oneTypeEntities in affectedEntities)
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

                    if (!storage.ValidateEntity(newItem))
                    {
                        return false;
                    }

                    preparedObjects.Add(newStorageItem.GlobalId, newItem);
                }
            }

            return true;
        }

        public Dictionary<ModelCode, List<ModelCode>> GetNeededProperties()
        {
            Dictionary<ModelCode, HashSet<ModelCode>> neededPropeties = new Dictionary<ModelCode, HashSet<ModelCode>>();
            foreach (var storageItemCreator in storageItemCreators.Values)
            {
                Dictionary<ModelCode, List<ModelCode>> propertyPerCreator = storageItemCreator.GetNeededProperties();

                foreach (var properties in propertyPerCreator)
                {
                    if (!neededPropeties.ContainsKey(properties.Key))
                    {
                        neededPropeties[properties.Key] = new HashSet<ModelCode>(properties.Value);
                    }
                    else
                    {
                        HashSet<ModelCode> hashSet = neededPropeties[properties.Key];
                        foreach (var property in properties.Value)
                        {
                            hashSet.Add(property);
                        }
                    }
                }
            }

            Dictionary<ModelCode, List<ModelCode>> returnValue = new Dictionary<ModelCode, List<ModelCode>>();

            foreach (var property in neededPropeties)
            {
                returnValue[property.Key] = property.Value.ToList();
            }

            return returnValue;
        }
    }
}
