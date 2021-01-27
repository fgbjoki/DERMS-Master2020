using Common.AbstractModel;
using Common.Communication;
using Common.GDA;
using Common.ServiceInterfaces;
using Common.ServiceInterfaces.Transaction;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Common.ComponentStorage
{
    public class TransactionManager : ITransaction, IModelPromotionParticipant
    {
        private string serviceName;
        private string serviceEndpoint;

        private List<IStorageTransactionProcessor> transactionProcessors;

        private Dictionary<ModelCode, List<ModelCode>> neededProperties;

        private GDAProxy gdaProxy;

        private Dictionary<DMSType, HashSet<long>> newNeededGids;

        public TransactionManager(string serviceName, string serviceEndpoint)
        {
            this.serviceName = serviceName;
            this.serviceEndpoint = serviceEndpoint;

            gdaProxy = new GDAProxy("gdaQueryEndpoint");

            neededProperties = new Dictionary<ModelCode, List<ModelCode>>();
        }

        public bool ApplyChanges(List<long> insertedEntities, List<long> updatedEntities, List<long> deletedEntities)
        {
            newNeededGids = new Dictionary<DMSType, HashSet<long>>();
            Dictionary<DMSType, List<long>> groupedEntities = insertedEntities.GroupBy(x => (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(x)).ToDictionary(x => x.Key, x => x.ToList());

            foreach (var transactionProcessor in transactionProcessors)
            {
                if (!transactionProcessor.ApplyChanges(groupedEntities, newNeededGids))
                {
                    return false;
                }
            }

            return Enlist();
        }

        public bool Commit()
        {
            bool isSuccessful = true;
            foreach (var transactionProcessor in transactionProcessors)
            {
                isSuccessful &= transactionProcessor.Commit();
            }

            return isSuccessful;
        }

        public bool Prepare()
        {
            Dictionary<DMSType, List<ResourceDescription>> affectedEntities = GetNewDataFromNMS();

            foreach (var transactionProcessor in transactionProcessors)
            {
                if (!transactionProcessor.Prepare(affectedEntities))
                {
                    return false;
                }
            }

            return true;
        }

        public bool Rollback()
        {
            bool isSuccessful = true;
            foreach (var transactoinProcessor in transactionProcessors)
            {
                isSuccessful &= transactoinProcessor.Rollback();
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
                WCFClient<ITransactionManager> transactionManager = new WCFClient<ITransactionManager>("transactionManagerEndpoint");

                return transactionManager.Proxy.EnlistService(serviceName, serviceEndpoint);
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private Dictionary<DMSType, List<ResourceDescription>> GetNewDataFromNMS()
        {
            Dictionary<DMSType, List<ResourceDescription>> nmsData = new Dictionary<DMSType, List<ResourceDescription>>(neededProperties.Count);

            foreach (var typeProperties in neededProperties)
            {
                List<ResourceDescription> rds = gdaProxy.GetExtentValues(typeProperties.Key, typeProperties.Value, newNeededGids[ModelCodeHelper.GetTypeFromModelCode(typeProperties.Key)].ToList());

                if (rds?.Count > 0)
                {
                    DMSType dmsType = ModelCodeHelper.GetTypeFromModelCode(typeProperties.Key);
                    nmsData[dmsType] = rds;
                }
            }

            return nmsData;
        }

        private void LoadNeededModelCodeProperties()
        {
            Dictionary<ModelCode, HashSet<ModelCode>> distinctProperties = new Dictionary<ModelCode, HashSet<ModelCode>>();
            foreach (var transactionProcessor in transactionProcessors)
            {
                Dictionary<ModelCode, List<ModelCode>> propertiesPerProcessor = transactionProcessor.GetNeededProperties();

                foreach (var properties in propertiesPerProcessor)
                {
                    if (!distinctProperties.ContainsKey(properties.Key))
                    {
                        distinctProperties[properties.Key] = new HashSet<ModelCode>(properties.Value);
                    }
                    else
                    {
                        HashSet<ModelCode> hashSet = distinctProperties[properties.Key];
                        foreach (var property in properties.Value)
                        {
                            hashSet.Add(property);
                        }
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
