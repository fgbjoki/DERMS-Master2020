using CalculationEngine.Graphs;
using CalculationEngine.Model.Topology;
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

namespace CalculationEngine
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class CalculationEngine : ITransaction, IModelPromotionParticipant, ISchemaRepresentation
    {
        private readonly string serviceName = "Calculation Engine";
        private string serviceUrlForTransaction;

        private ModelResourcesDesc modelResourcesDesc;

        private GraphBranchManipulator graphManipulator;

        private BreakerMessageMapping breakerMessageMapping;

        private TopologyStorage topologyStorage;

        private TransactionManager transactionManager;

        private TopologyAnalysis.TopologyAnalysis topologyAnalysis;

        private SchemaRepresentation schemaRepresentation;

        private IDynamicPublisher dynamicPublisher;

        public CalculationEngine()
        {
            InternalCEInitialization();

            InitializeDynamicPublisher();

            InitializeGraphs();

            InitializeStorages();

            InitializeForTransaction();
        }

        private void InitializeGraphs()
        {
            topologyAnalysis = new TopologyAnalysis.TopologyAnalysis();
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

        private void InternalCEInitialization()
        {
            LoadConfigurationFromAppConfig();

            breakerMessageMapping = new BreakerMessageMapping();
            graphManipulator = new GraphBranchManipulator();
            modelResourcesDesc = new ModelResourcesDesc();
        }

        private void InitializeStorages()
        {
            GraphsCreationProcessor graphsCreationProcessor = new GraphsCreationProcessor(modelResourcesDesc, schemaRepresentation, topologyAnalysis);
            topologyStorage = new TopologyStorage(breakerMessageMapping, graphManipulator, graphsCreationProcessor, modelResourcesDesc);
        }

        private void InitializeForTransaction()
        {     
            transactionManager = new TransactionManager(serviceName, serviceUrlForTransaction);
            transactionManager.LoadTransactionProcessors(new List<ITransactionStorage>() { topologyStorage });
        }

        private void InitializeDynamicPublisher()
        {
            dynamicPublisher = new DynamicPublisher(serviceName);
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

        public IEnumerable<long> GetSchemaSources()
        {
            return schemaRepresentation.GetSchemaSources();
        }

        public SchemaGraphChanged GetSchema(long sourceId)
        {
            return schemaRepresentation.GetSchema(sourceId);
        }
    }
}
