using CalculationEngine.Graphs.ConnectivityGraphCreation.GraphRules;
using CalculationEngine.Model.Topology.Graph;
using CalculationEngine.Model.Topology.Graph.Connectivity;
using CalculationEngine.Model.Topology.Transaction;
using Common.AbstractModel;
using Common.ComponentStorage;
using System;
using System.Collections.Generic;

namespace CalculationEngine.Graphs.ConnectivityGraphCreation
{
    public class ConnectivityGraphCreator
    {
        private IGraphRule graphRules;
        private GraphBranchManipulator graphManipulator;
        private ConnectivityObjectTraverser connectivityNodeTraverser;
        private IConnectivityObjectTraverser objectTraverser;

        public ConnectivityGraphCreator(GraphBranchManipulator graphManipulator, ModelResourcesDesc modelResourceDesc)
        {
            this.graphManipulator = graphManipulator;

            graphRules = new ConnectivityGraphRuleApplier(graphManipulator);
            connectivityNodeTraverser = new ConnectivityNodeTraverser();
            objectTraverser = new ConnectivityObjectExplorer(modelResourceDesc);
        }

        public List<ConnectivityGraph> CreateGraph(IStorage<EnergySource> energySources)
        {
            ConnectivityGraph currentGraph = null;
            List<ConnectivityGraph> graphs = new List<ConnectivityGraph>(1);
            Queue<ConnectivityNodeTraverseWrapper> objectsToProcess = new Queue<ConnectivityNodeTraverseWrapper>();

            foreach (var energySource in energySources.GetAllEntities())
            {
                if (!EnergySourceExistsInGraphs(graphs, energySource.GlobalId))
                {
                    currentGraph = new ConnectivityGraph();
                    graphs.Add(currentGraph);
                }
                else
                {
                    continue;
                }

                ConnectivityNodeTraverseWrapper rootWrapper = new ConnectivityNodeTraverseWrapper(null, energySource);

                PreProcessRoot(rootWrapper, currentGraph, objectsToProcess);

                while (objectsToProcess.Count > 0)
                {
                    ConnectivityNodeTraverseWrapper currentElement = objectsToProcess.Dequeue();

                    ProcessChildren(currentElement, currentGraph, objectsToProcess);
                }
            }

            ApplyRules(graphs);

            return graphs;
        }

        private void ApplyRules(List<ConnectivityGraph> graphs)
        {
            foreach (var graph in graphs)
            {
                graphRules.ApplyRule(graph);
            }
        }

        private void PreProcessRoot(ConnectivityNodeTraverseWrapper root, ConnectivityGraph graph, Queue<ConnectivityNodeTraverseWrapper> objectsToProcess)
        {
            ConnectivityGraphNode rootNode = new ConnectivityGraphNode(root.TopologyObject.GlobalId);

            graph.AddNode(rootNode);

            objectsToProcess.Enqueue(root);        
        }

        private void ProcessChildren(ConnectivityNodeTraverseWrapper currentElement, ConnectivityGraph graph, Queue<ConnectivityNodeTraverseWrapper> objectsToProcess)
        {
            ConnectivityGraphNode currentNode = graph.GetNode(currentElement.TopologyObject.GlobalId);

            List<ConnectivityNodeTraverseWrapper> neighbours = objectTraverser.ExploreNeighbourObjects(currentElement);

            foreach (var neighbour in neighbours)
            {
                ConnectivityGraphNode newNode = new ConnectivityGraphNode(neighbour.TopologyObject.GlobalId);

                if (!graph.AddNode(newNode))
                {
                    throw new Exception($"Graph couldn't be created, there is a circular dependence in the graph.");
                }

                graphManipulator.AddBranch(currentNode, newNode);

                objectsToProcess.Enqueue(neighbour);
            }
        }

        private bool EnergySourceExistsInGraphs(List<ConnectivityGraph> graphs, long energySourceGlobalId)
        {
            if (graphs?.Count == 0)
            {
                return false;
            }

            foreach (var graph in graphs)
            {
                if (graph.NodeExists(energySourceGlobalId))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
