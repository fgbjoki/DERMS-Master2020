using Common.ComponentStorage;
using Common.ServiceInterfaces;
using Common.ServiceInterfaces.Transaction;
using Common.ServiceInterfaces.UIAdapter.SummaryJobs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using UIAdapter.TransactionProcessing.Storages;
using Common.UIDataTransferObject.RemotePoints;
using UIAdapter.SummaryJobs;
using Common.PubSub;
using UIAdapter.PubSub.DynamicListeners;
using UIAdapter.Schema;
using UIAdapter.TransactionProcessing.Storages.Schema;
using Common.ServiceInterfaces.UIAdapter;
using Common.UIDataTransferObject.Schema;
using NServiceBus;
using Common.UIDataTransferObject.DERGroup;

namespace UIAdapter
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class UIAdapter : ITransaction, IModelPromotionParticipant, IAnalogRemotePointSummaryJob, IDiscreteRemotePointSummaryJob, ISchema, IDERGroupSummaryJob
    {
        private readonly string serviceName = "UIAdapter";
        private string serviceUrlForTransaction;

        private TransactionManager transactionManager;

        private EnergySourceStorage schemaEnergySourceStorage;
        private BreakerStorage schemaBreakerStorage;

        private AnalogRemotePointStorage analogRemotePointStorage;
        private DiscreteRemotePointStorage discreteRemotePointStorage;

        private TransactionProcessing.Storages.DERs.GeneratorStorage generatorStorage;
        private TransactionProcessing.Storages.DERGroup.DERGroupStorage derGroupStorage;
        private TransactionProcessing.Storages.DERs.EnergyStorageStorage energyStorageStorage;

        private DERGroupSummaryJob derGroupSummaryJob;
        private AnalogRemotePointSummaryJob analogRemotePointSummaryJob;
        private DiscreteRemotePointSummaryJob discreteRemotePointSummaryJob;

        private DynamicListenersManager dynamicListenerManager;

        private SchemaRepresentation schemaRepresentation;

        public UIAdapter()
        {
            LoadConfigurationFromAppConfig();

            transactionManager = new TransactionManager(serviceName, serviceUrlForTransaction);

            InitializeTransactionStorages();

            InitializeSchemaRepresentation();

            InitializeJobs();

            InitializePubSub();
        }

        private void InitializeSchemaRepresentation()
        {
            schemaRepresentation = new SchemaRepresentation(schemaEnergySourceStorage, schemaBreakerStorage);
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
        

        private void InitializeJobs()
        {
            derGroupSummaryJob = new DERGroupSummaryJob(derGroupStorage);
            analogRemotePointSummaryJob = new AnalogRemotePointSummaryJob(analogRemotePointStorage);
            discreteRemotePointSummaryJob = new DiscreteRemotePointSummaryJob(discreteRemotePointStorage);
        }

        private void InitializeDynamicHandlers()
        {
            dynamicListenerManager.ConfigureSubscriptions(derGroupStorage.GetSubscriptions());
            dynamicListenerManager.ConfigureSubscriptions(analogRemotePointStorage.GetSubscriptions());
            dynamicListenerManager.ConfigureSubscriptions(discreteRemotePointStorage.GetSubscriptions());
            dynamicListenerManager.ConfigureSubscriptions(schemaRepresentation.GetSubscriptions());
        }

        private void InitializeDynamicListeners()
        {
            dynamicListenerManager = new DynamicListenersManager(serviceName);
            List<IDynamicListener> listeners = new List<IDynamicListener>()
            {
                new AnalogRemotePointChangedListener(),
                new DiscreteRemotePointChangedListener(),
                new DERStateChangedListener(),
                new EnergyBalanceChangedListener()
            };

            foreach (var listener in listeners)
            {
                dynamicListenerManager.AddDynamicHandlers(listener.Topic, listener);
            }
        }

        private void InitializePubSub()
        {
            InitializeDynamicListeners();
            InitializeDynamicHandlers();

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration(serviceName);
            dynamicListenerManager.StartListening(endpointConfiguration);
        }

        private void InitializeTransactionStorages()
        {
            analogRemotePointStorage = new AnalogRemotePointStorage();
            discreteRemotePointStorage = new DiscreteRemotePointStorage();

            schemaEnergySourceStorage = new EnergySourceStorage();
            schemaBreakerStorage = new BreakerStorage();

            generatorStorage = new TransactionProcessing.Storages.DERs.GeneratorStorage();
            energyStorageStorage = new TransactionProcessing.Storages.DERs.EnergyStorageStorage();

            derGroupStorage = new TransactionProcessing.Storages.DERGroup.DERGroupStorage();

            transactionManager.LoadTransactionProcessors(new List<ITransactionStorage>() { analogRemotePointStorage, discreteRemotePointStorage, schemaEnergySourceStorage, schemaBreakerStorage, energyStorageStorage, generatorStorage, derGroupStorage });
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

            serviceUrlForTransaction = serviceSection.Services[0].Host.BaseAddresses[0].BaseAddress + transactionAddition;
        }

        public List<AnalogRemotePointSummaryDTO> GetAllAnalogEntities()
        {
            return analogRemotePointSummaryJob.GetAllEntities();
        }

        public List<DiscreteRemotePointSummaryDTO> GetAllDiscreteEntities()
        {
            return discreteRemotePointSummaryJob.GetAllEntities();
        }
        
        public SubSchemaDTO GetSchema(long energySourceId)
        {
            return schemaRepresentation.GetSchema(energySourceId);
        }

        public SubSchemaConductingEquipmentEnergized GetEquipmentStates(long energySourceId)
        {
            return schemaRepresentation.GetEquipmentStates(energySourceId);
        }

        public List<EnergySourceDTO> GetSubstations()
        {
            return schemaRepresentation.GetSubstations();
        }

        public AnalogRemotePointSummaryDTO GetEntity(long globalId)
        {
            return analogRemotePointSummaryJob.GetEntity(globalId);
        }

        DiscreteRemotePointSummaryDTO IDiscreteRemotePointSummaryJob.GetEntity(long globalId)
        {
            return discreteRemotePointSummaryJob.GetEntity(globalId);
        }

        List<DERGroupSummaryDTO> IDERGroupSummaryJob.GetAllAnalogEntities()
        {
            return derGroupSummaryJob.GetAllEntities();
        }

        DERGroupSummaryDTO IDERGroupSummaryJob.GetEntity(long globalId)
        {
            return derGroupSummaryJob.GetEntity(globalId);
        }

        public SchemaEnergyBalanceDTO GetEnergyBalance(long energySourceGid)
        {
            return schemaRepresentation.GetEnergyBalance(energySourceGid);
        }
    }
}
