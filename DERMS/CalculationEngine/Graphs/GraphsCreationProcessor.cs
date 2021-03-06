﻿using System;
using CalculationEngine.Graphs.ConnectivityGraphCreation;
using CalculationEngine.Graphs.SchemaGraphCreation;
using CalculationEngine.Graphs.TopologyGraphCreation;
using CalculationEngine.Model.Topology.Graph;
using CalculationEngine.TransactionProcessing.Storage;
using CalculationEngine.TransactionProcessing.Storage.Topology;
using Common.AbstractModel;
using CalculationEngine.Graphs.GraphProcessors;
using CalculationEngine.Model.Topology.Graph.Schema;
using CalculationEngine.Model.Topology.Graph.Topology;
using System.Collections.Generic;
using CalculationEngine.Model.Topology.Graph.Connectivity;
using Common.Logger;
using Common.ComponentStorage;
using CalculationEngine.Model.Topology.Transaction;
using System.Threading;

namespace CalculationEngine.Graphs
{
    public class GraphsCreationProcessor : IStorageDependentUnit<TopologyStorage>
    {
        private SchemaGraphCreator schemaGraphCreator;

        private TopologyGraphCreator topologyGraphCreator;

        private ConnectivityGraphCreator connectivityGraphCreator;

        private List<ISchemaGraph> schemaGraphs;
        private List<IMultipleRootGraph<TopologyGraphNode>> topologyGraphs;

        private IGraphProcessor<ISchemaGraph> schema;
        private IGraphProcessor<TopologyGraph> topologyAnalysis;

        private IStorage<DiscreteRemotePoint> discreteRemotePointStorage;

        private AutoResetEvent commitedEvent;

        public GraphsCreationProcessor(ModelResourcesDesc modelResDesc, IStorage<DiscreteRemotePoint> discreteRemotePointStorage, IGraphProcessor<ISchemaGraph> schema, IGraphProcessor<TopologyGraph> topologyAnalysis)
        {
            this.schema = schema;
            this.topologyAnalysis = topologyAnalysis;
            this.discreteRemotePointStorage = discreteRemotePointStorage;

            commitedEvent = new AutoResetEvent(false);
            topologyAnalysis.AlignEvent = commitedEvent;

            schemaGraphCreator = new SchemaGraphCreator();
            topologyGraphCreator = new TopologyGraphCreator(new TopologyGraphBranchManipulator(), new TopologyBreakerGraphBranchManipulator());
            connectivityGraphCreator = new ConnectivityGraphCreator(new GraphBranchManipulator(), modelResDesc);
        }
        
        public bool Prepare(TopologyStorage storage)
        {
            try
            {
                List<ConnectivityGraph> connectivityGraphs = connectivityGraphCreator.CreateGraph(storage.EnergySourceStorage);

                topologyGraphs = CreateGraphsTopologyGraphs(connectivityGraphs);
                schemaGraphs = CreateSchemaGraphs(connectivityGraphs);

                return true;
            }
            catch (Exception e)
            {
                Logger.Instance.Log($"[{this.GetType()}] Failed to prepare graphs. More info: {e.Message}\nStack trace: {e.StackTrace}");
                return false;
            }
        }

        public bool Commit()
        {
            bool commited = true;
            try
            {
                foreach (var topologyGraph in topologyGraphs)
                {
                    topologyAnalysis.AddGraph(topologyGraph as TopologyGraph);
                }

                commitedEvent.Set();

                foreach (var schemaGraph in schemaGraphs)
                {
                    schema.AddGraph(schemaGraph);
                }

            }
            catch (Exception e)
            {
                Logger.Instance.Log($"[{this.GetType()}] Failed to commit graphs. More info: {e.Message}\nStack trace: {e.StackTrace}");

                commited = false;
            }

            DisposeTransactionResources();

            return commited;
        }

        public bool Rollback()
        {
            DisposeTransactionResources();

            return true;
        }

        private List<IMultipleRootGraph<TopologyGraphNode>> CreateGraphsTopologyGraphs(List<ConnectivityGraph> connectivityGraphs)
        {
            List<IMultipleRootGraph<TopologyGraphNode>> topologyGraphs = new List<IMultipleRootGraph<TopologyGraphNode>>(connectivityGraphs.Count);

            foreach (var connectivityGraph in connectivityGraphs)
            {
                topologyGraphs.AddRange(topologyGraphCreator.CreateGraph(connectivityGraph));
            }

            return topologyGraphs;
        }

        private List<ISchemaGraph> CreateSchemaGraphs(List<ConnectivityGraph> connectivityGraphs)
        {
            List<ISchemaGraph> schemaGraphs = new List<ISchemaGraph>(connectivityGraphs.Count);

            foreach (var connectivityGraph in connectivityGraphs)
            {
                schemaGraphs.AddRange(schemaGraphCreator.CreateGraph(connectivityGraph));
            }

            return schemaGraphs;
        }

        private void DisposeTransactionResources()
        {
            schemaGraphs.Clear();
            topologyGraphs.Clear();

            schemaGraphs = null;
            topologyGraphs = null;
        }
    }
}
