using System;
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

namespace CalculationEngine.Graphs
{
    public class GraphsCreationProcessor : IStorageDependentUnit<TopologyStorage>
    {
        private SchemaGraphCreator schemaGraphCreator;

        private TopologyGraphCreator topologyGraphCreator;

        private ConnectivityGraphCreator connectivityGraphCreator;

        private List<ISingleRootGraph<SchemaGraphNode>> schemaGraphs;
        private List<IMultipleRootGraph<TopologyGraphNode>> topologyGraphs;

        private IGraphProcessor<ISingleRootGraph<SchemaGraphNode>> schema;
        private IGraphProcessor<IMultipleRootGraph<TopologyGraphNode>> topologyAnalysis;

        public GraphsCreationProcessor(ModelResourcesDesc modelResDesc, IGraphProcessor<ISingleRootGraph<SchemaGraphNode>> schema, IGraphProcessor<IMultipleRootGraph<TopologyGraphNode>> topologyAnalysis)
        {
            this.schema = schema;
            this.topologyAnalysis = topologyAnalysis;

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
                    topologyAnalysis.AddGraph(topologyGraph);
                }

                foreach (var schemaGraph in schemaGraphs)
                {
                    schema.AddGraph(schemaGraph);
                }

            }
            catch (Exception e)
            {
                Logger.Instance.Log($"[{this.GetType()}] Failed to commit graphs. More info: {e.Message}");

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

        private List<ISingleRootGraph<SchemaGraphNode>> CreateSchemaGraphs(List<ConnectivityGraph> connectivityGraphs)
        {
            List<ISingleRootGraph<SchemaGraphNode>> schemaGraphs = new List<ISingleRootGraph<SchemaGraphNode>>(connectivityGraphs.Count);

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
