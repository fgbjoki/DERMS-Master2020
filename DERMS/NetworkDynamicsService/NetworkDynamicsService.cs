using Common.ServiceInterfaces.NetworkDynamicsService;
using System;
using System.Collections.Generic;
using Common.SCADA;
using System.ServiceModel;
using NetworkDynamicsService.FieldValueProcessing;
using Common.ComponentStorage;
using NetworkDynamicsService.TransactionProcessing.Storages;
using Common.PubSub;
using System.ServiceModel.Configuration;
using System.Configuration;
using Common.ServiceInterfaces.Transaction;
using Common.ServiceInterfaces;

namespace NetworkDynamicsService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class NetworkDynamicsService : IFieldValuesProcessing, ITransaction, IModelPromotionParticipant
    {
        private readonly string serviceName = "NetworkDynamicsService";
        private string serviceUrl;

        private AnalogRemotePointStorage analogRemotePointStorage;
        private DiscreteRemotePointStorage discreteRemotePointStorage;

        private IDynamicPublisher dynamicPublisher;

        private IFieldValuesProcessing fieldValueProcessingUnit;

        private TransactionManager transactionManager;

        public NetworkDynamicsService()
        {
            LoadConfigurationFromAppConfig();

            dynamicPublisher = new DynamicPublisher(serviceName);

            InitializeStorages();

            InitializeTransaction();

            fieldValueProcessingUnit = new FieldValueProcessor(analogRemotePointStorage, discreteRemotePointStorage, dynamicPublisher);
        }

        public void ProcessFieldValues(IEnumerable<RemotePointFieldValue> fieldValues)
        {
            fieldValueProcessingUnit.ProcessFieldValues(fieldValues);
        }

        public bool Prepare()
        {
            return transactionManager.Prepare();
        }

        public bool Commit()
        {
            return transactionManager.Commit();
        }

        public bool Rollback()
        {
            return transactionManager.Rollback();
        }

        public bool ApplyChanges(List<long> insertedEntities, List<long> updatedEntities, List<long> deletedEntities)
        {
            return transactionManager.ApplyChanges(insertedEntities, updatedEntities, deletedEntities);
        }

        private void LoadConfigurationFromAppConfig()
        {
            ServicesSection serviceSection = ConfigurationManager.GetSection("system.serviceModel/services") as ServicesSection;
            ServiceEndpointElementCollection endpoints = serviceSection.Services[0].Endpoints;
            string transactionAddition = String.Empty;
            for (int i = 0; i < endpoints.Count; i++)
            {
                ServiceEndpointElement endpoint = endpoints[i];
                if (endpoint.Contract.Equals(typeof(ITransaction).ToString()))
                {
                    transactionAddition = $"/{endpoint.Address.OriginalString}";
                }
            }

            serviceUrl = serviceSection.Services[0].Host.BaseAddresses[0].BaseAddress + transactionAddition;
        }

        private void InitializeTransaction()
        {
            transactionManager = new TransactionManager(serviceName, serviceUrl);

            transactionManager.LoadTransactionProcessors(new List<ITransactionStorage>() { analogRemotePointStorage, discreteRemotePointStorage });
        }

        private void InitializeStorages()
        {
            analogRemotePointStorage = new AnalogRemotePointStorage();
            discreteRemotePointStorage = new DiscreteRemotePointStorage();
        }
    }
}
