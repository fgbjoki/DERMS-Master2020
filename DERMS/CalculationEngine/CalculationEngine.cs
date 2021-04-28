using CalculationEngine.Graphs;
using CalculationEngine.Model.Topology.Graph;
using CalculationEngine.Schema;
using CalculationEngine.TransactionProcessing.Storage.Topology;
using Common.AbstractModel;
using Common.ComponentStorage;
using Common.PubSub;
using Common.ServiceInterfaces;
using Common.ServiceInterfaces.CalculationEngine;
using Common.ServiceInterfaces.Transaction;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using Common.DataTransferObjects.CalculationEngine;
using CalculationEngine.TransactionProcessing.Storage;
using CalculationEngine.PubSub.DynamicListeners;
using CalculationEngine.TransactionProcessing.Storage.EnergyBalance;
using CalculationEngine.EnergyCalculators;
using NServiceBus;
using Common.Helpers.Breakers;
using CalculationEngine.TransactionProcessing.Storage.DERStates;
using CalculationEngine.DERStates;
using CalculationEngine.Commanding.BreakerCommanding;

namespace CalculationEngine
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class CalculationEngine : ITransaction, IModelPromotionParticipant, ISchemaRepresentation, IBreakerCommanding
    {
        private readonly string serviceName = "Calculation Engine";
        private string serviceUrlForTransaction;

        private ModelResourcesDesc modelResourcesDesc;

        private GraphBranchManipulator graphManipulator;

        private BreakerMessageMapping breakerMessageMapping;

        private TopologyStorage topologyStorage;

        private EnergyBalanceStorage energyBalanceStorage;

        private TransactionManager transactionManager;

        private TopologyAnalysis.TopologyAnalysis topologyAnalysis;
        private DiscreteRemotePointStorage discreteRemotePointStorage;

        private DERStateStorage derStateStorage;

        private SchemaRepresentation schemaRepresentation;

        private DynamicPublisher dynamicPublisher;

        private DynamicListenersManager dynamicListenersManager;

        private EnergyBalanceCalculator energyBalanceCalculator;
        private DERStateDeterminator derStateDeterminator;

        private BreakerCommandingUnit breakerCommandingValidator;

        private EndpointConfiguration endpointConfiguration;

        public CalculationEngine()
        {
            InternalCEInitialization();

            endpointConfiguration = InitializeDynamicPublisher();

            InitializeGraphs();

            InitializeStorages();

            InitializeForTransaction();

            energyBalanceCalculator = new EnergyBalanceCalculator(energyBalanceStorage, topologyAnalysis, dynamicPublisher);
            derStateDeterminator = new DERStateDeterminator(energyBalanceStorage.EnergySourceStorage, derStateStorage, topologyAnalysis, dynamicPublisher);

            InitializePubSub();
        }

        private void InitializeGraphs()
        {
            discreteRemotePointStorage = new DiscreteRemotePointStorage();

            topologyAnalysis = new TopologyAnalysis.TopologyAnalysis(discreteRemotePointStorage, breakerMessageMapping);
            breakerCommandingValidator = new BreakerCommandingUnit(topologyAnalysis, discreteRemotePointStorage, breakerMessageMapping);
            topologyAnalysis.BreakerLoopCommandingValidator = breakerCommandingValidator;

            schemaRepresentation = new SchemaRepresentation();
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

        public IEnumerable<long> GetSchemaSources()
        {
            return schemaRepresentation.GetSchemaSources();
        }

        public SchemaGraphChanged GetSchema(long sourceId)
        {
            return schemaRepresentation.GetSchema(sourceId);
        }

        private void InternalCEInitialization()
        {
            LoadConfigurationFromAppConfig();

            breakerMessageMapping = new BreakerMessageMapping();
            graphManipulator = new GraphBranchManipulator();
            modelResourcesDesc = new ModelResourcesDesc();
        }

        private void InitializeStorages()
        {
            GraphsCreationProcessor graphsCreationProcessor = new GraphsCreationProcessor(modelResourcesDesc, discreteRemotePointStorage, schemaRepresentation, topologyAnalysis);
            topologyStorage = new TopologyStorage(breakerMessageMapping, graphManipulator, graphsCreationProcessor, modelResourcesDesc);

            energyBalanceStorage = new EnergyBalanceStorage();

            derStateStorage = new DERStateStorage();
        }

        private void InitializeForTransaction()
        {     
            transactionManager = new TransactionManager(serviceName, serviceUrlForTransaction);
            transactionManager.LoadTransactionProcessors(new List<ITransactionStorage>() { discreteRemotePointStorage, topologyStorage, energyBalanceStorage, derStateStorage });
        }

        private EndpointConfiguration InitializeDynamicPublisher()
        {
            dynamicPublisher = new DynamicPublisher();
            return dynamicPublisher.ConfigureEndpointInstance(serviceName);
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

        private void InitializePubSub()
        {           
            InitializeDynamicListeners();
            InitializeDynamicHandlers();

            IEndpointInstance endpointInstance = dynamicListenersManager.StartListening(endpointConfiguration);
            dynamicPublisher.EndpointInstance = endpointInstance;
        }

        private void InitializeDynamicHandlers()
        {
            dynamicListenersManager.ConfigureSubscriptions(discreteRemotePointStorage.GetSubscriptions());
            dynamicListenersManager.ConfigureSubscriptions(topologyAnalysis.GetSubscriptions());
            dynamicListenersManager.ConfigureSubscriptions(energyBalanceCalculator.GetSubscriptions());
            dynamicListenersManager.ConfigureSubscriptions(derStateDeterminator.GetSubscriptions());
        }

        private void InitializeDynamicListeners()
        {
            dynamicListenersManager = new DynamicListenersManager("Calculation Engine");

            List<IDynamicListener> listeners = new List<IDynamicListener>()
            {
                new AnalogRemotePointChangedListener(),
                new DiscreteRemotePointChangedListener(),
            };

            foreach (var listener in listeners)
            {
                dynamicListenersManager.AddDynamicHandlers(listener.Topic, listener);
            }
        }

        public void UpdateBreakers()
        {
            throw new NotImplementedException();
        }

        public bool ValidateCommand(long breakerGid, BreakerState breakerState)
        {
            return breakerCommandingValidator.ValidateCommand(breakerGid, breakerState);
        }

        public bool SendCommand(long breakerGid, int breakerValue)
        {
            return breakerCommandingValidator.SendCommand(breakerGid, breakerValue);
        }
    }
}
