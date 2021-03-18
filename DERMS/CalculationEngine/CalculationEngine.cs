﻿using CalculationEngine.Graphs.ConnectivityGraphCreation;
using CalculationEngine.Model.Topology;
using CalculationEngine.Model.Topology.Graph;
using CalculationEngine.Schema;
using CalculationEngine.TransactionProcessing.Storage.Topology;
using Common.ComponentStorage;
using Common.ServiceInterfaces;
using Common.ServiceInterfaces.Transaction;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Configuration;

namespace CalculationEngine
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class CalculationEngine : ITransaction, IModelPromotionParticipant
    {
        private readonly string serviceName = "Calculation Engine";
        private string serviceUrlForTransaction;

        private GraphBranchManipulator graphManipulator;

        private BreakerMessageMapping breakerMessageMapping;

        private TopologyStorage topologyStorage;

        private TransactionManager transactionManager;

        private TopologyAnalysis.TopologyAnalysis topologyAnalysis;

        private SchemaRepresentation schemaRepresentation;

        public CalculationEngine()
        {
            InternalCEInitialization();

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
        }

        private void InitializeStorages()
        {
            topologyStorage = new TopologyStorage(breakerMessageMapping, graphManipulator, topologyAnalysis, schemaRepresentation);
        }

        private void InitializeForTransaction()
        {     
            transactionManager = new TransactionManager(serviceName, serviceUrlForTransaction);
            transactionManager.LoadTransactionProcessors(new List<ITransactionStorage>() { topologyStorage });
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
    }
}
