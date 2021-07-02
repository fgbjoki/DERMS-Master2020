using Core.Common.AbstractModel;
using Common.Communication;
using Core.Common.GDA;
using Core.Common.ServiceInterfaces.Transaction;
using System;
using System.Linq;
using System.Collections.Generic;
using Core.Common.ServiceInterfaces.NMS;
using Core.Common.Transaction.StorageTransactionProcessor;
using Core.Common.Transaction.Storage;
using Core.Common.ReliableCollectionProxy;
using Microsoft.ServiceFabric.Data;
using Core.Common.PropertyWrappers.EnumWrappers;

namespace Core.Common.Transaction
{
    public class TransactionManager : ServiceInterfaces.Transaction.ITransaction, IModelPromotionParticipant
    {
        private string serviceName;
        private string serviceEndpoint;

        private List<IStorageTransactionProcessor> transactionProcessors;

        private Dictionary<DMSType, List<ModelCode>> neededProperties;

        private GDAProxy gdaProxy;

        private readonly string newNeededGidsDictionary = "newNeededGids";

        private IReliableStateManager stateManager;

        private Action<string> log;

        public TransactionManager(IReliableStateManager stateManager, string serviceName, string serviceEndpoint, Action<string> log)
        {
            this.log = log;
            this.serviceName = serviceName;
            this.stateManager = stateManager;
            this.serviceEndpoint = serviceEndpoint;

            gdaProxy = new GDAProxy();
            neededProperties = new Dictionary<DMSType, List<ModelCode>>();
        }

        public bool ApplyChanges(List<long> insertedEntities, List<long> updatedEntities, List<long> deletedEntities)
        {
            log($"[{GetType().Name}] Apply changes called");
            ReliableDictionaryProxy.CreateDictionary<HashSet<long>, int>(stateManager, newNeededGidsDictionary);
            Dictionary<DMSType, List<long>> groupedEntities = insertedEntities.GroupBy(x => (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(x)).ToDictionary(x => x.Key, x => x.ToList());

            log($"[{GetType().Name}] Applying changes on each transaction processor");
            foreach (var transactionProcessor in transactionProcessors)
            {
                if (!transactionProcessor.ApplyChanges(groupedEntities, stateManager))
                {
                    return false;
                }
            }
            log($"[{GetType().Name}] Apply changes finished");
            return Enlist();
        }

        public bool Commit()
        {
            log($"[{GetType().Name}] Commit called");
            bool isSuccessful = true;
            foreach (var transactionProcessor in transactionProcessors)
            {
                isSuccessful &= transactionProcessor.Commit(stateManager);
            }

            log($"[{GetType().Name}] Commit successful: " + isSuccessful);

            return isSuccessful;
        }

        public bool Prepare()
        {
            log($"[{GetType().Name}] Prepare called");
            Dictionary<DMSType, List<ResourceDescription>> affectedEntities = GetNewDataFromNMS();

            foreach (var transactionProcessor in transactionProcessors)
            {
                if (!transactionProcessor.Prepare(stateManager, affectedEntities))
                {
                    log($"[{GetType().Name}] Prepare failed");
                    return false;
                }
            }

            log($"[{GetType().Name}] Prepare succeeded");
            return true;
        }

        public bool Rollback()
        {
            log($"[{GetType().Name}] Rollback called");
            bool isSuccessful = true;
            foreach (var transactoinProcessor in transactionProcessors)
            {
                isSuccessful &= transactoinProcessor.Rollback(stateManager);
            }

            return isSuccessful;
        }

        public void LoadTransactionProcessors(List<ITransactionStorage> storages)
        {
            transactionProcessors = new List<IStorageTransactionProcessor>(storages.Count);
            foreach (var storage in storages)
            {
                transactionProcessors.AddRange(storage.GetStorageTransactionProcessors());
            }

            LoadNeededModelCodeProperties();
        }

        private bool Enlist()
        {
            try
            {
                log($"[{GetType().Name}] Enlisting service into transaction");
                WCFClient<ITransactionManager> transactionManager = new WCFClient<ITransactionManager>("transactionManagerEndpoint");

                return transactionManager.Proxy.EnlistService(serviceName, serviceEndpoint);
            }
            catch (Exception e)
            {
                log($"[{GetType().Name}] Error while enlisting service into transaction.");
                //Common.Logger.Logger.Instance.Log(e);
                return false;
            }
        }

        private Dictionary<DMSType, List<ResourceDescription>> GetNewDataFromNMS()
        {
            log($"[{GetType().Name}] Fetching data from NMS");
            Dictionary<DMSType, List<ResourceDescription>> nmsData = new Dictionary<DMSType, List<ResourceDescription>>(neededProperties.Count);

            foreach (var typeProperties in neededProperties)
            {
                List<ResourceDescription> rds = null;

                DMSType neededDMSType = typeProperties.Key;

                HashSet<long> neededGids = ReliableDictionaryProxy.GetEntity<HashSet<long>, int>(stateManager, (int)neededDMSType, newNeededGidsDictionary);
                if (neededGids == null)
                {
                    continue;
                }

                rds = gdaProxy.GetExtentValues(neededDMSType, typeProperties.Value, neededGids.ToList());

                if (rds?.Count > 0)
                {
                    nmsData[neededDMSType] = rds;
                }
            }

            log($"[{GetType().Name}] Data fetched from NMS");

            return nmsData;
        }

        private void LoadNeededModelCodeProperties()
        {
            Dictionary<DMSType, HashSet<ModelCode>> distinctProperties = new Dictionary<DMSType, HashSet<ModelCode>>();
            foreach (var transactionProcessor in transactionProcessors)
            {
                Dictionary<DMSType, List<ModelCode>> propertiesPerProcessor = transactionProcessor.GetNeededProperties();

                foreach (var properties in propertiesPerProcessor)
                {
                    if (!distinctProperties.ContainsKey(properties.Key))
                    {
                        distinctProperties[properties.Key] = new HashSet<ModelCode>(properties.Value);
                    }

                    HashSet<ModelCode> hashSet = distinctProperties[properties.Key];
                    foreach (var property in properties.Value)
                    {
                        hashSet.Add(property);
                    }
                }
            }

            foreach (var property in distinctProperties)
            {
                neededProperties[property.Key] = property.Value.ToList();
            }
        }
    }
}
