using CalculationEngine.Model.Topology.Graph;
using CalculationEngine.Model.Topology.Graph.Schema;
using System.Collections.Generic;
using CalculationEngine.Graphs.GraphReductionRules;
using CalculationEngine.Graphs.GraphReductionRules.Schema;
using CalculationEngine.Graphs.SchemaGraphCreation.Helpers;
using System.Linq;
using CalculationEngine.Model.Topology.Graph.Connectivity;

namespace CalculationEngine.Graphs.SchemaGraphCreation
{
    public class SchemaGraphCreator : BaseDepedentGraphCreator<ISchemaGraph, SchemaGraphNode, IMultipleRootGraph<ConnectivityGraphNode>, ConnectivityGraphNode>
    {
        private SchemaGraphBranchManipulator graphBranchManipulator;
        private ConnectivityBreakerNodeFinder connectivityBreakerFinder;

        public SchemaGraphCreator()
        {
            connectivityBreakerFinder = new ConnectivityBreakerNodeFinder();
            graphBranchManipulator = new SchemaGraphBranchManipulator();
        }

        protected override IEnumerable<GraphReductionRule<SchemaGraphNode>> GetReductionRules()
        {          
            return new List<GraphReductionRule<SchemaGraphNode>>()
            {
                new SchemaACLSBranchGraphRule(graphBranchManipulator),
            };
        }

        private SchemaGraphNode GetNode(ISingleRootGraph<SchemaGraphNode> graph, ConnectivityGraphNode oldNode)
        {
            SchemaGraphNode node = graph.GetNode(oldNode.Item);

            if (node == null)
            {
                node = CreateNewNode(oldNode);
                graph.AddNode(node);
            }

            return node;
        }

        private bool ShouldSkipNode(long connectivityBreakerGid, ConnectivityGraphNode currentNode)
        {
            return currentNode.Item == connectivityBreakerGid;
        }

        private void ApplyReductionRules(ISingleRootGraph<SchemaGraphNode> graph)
        {
            var root = graph.GetRoot();

            foreach (var reductionRule in GetReductionRules())
            {
                Queue<SchemaGraphNode> nodesToProcess = new Queue<SchemaGraphNode>();
                nodesToProcess.Enqueue(root);

                SchemaGraphNode currentNode;

                while (nodesToProcess.Count > 0)
                {
                    currentNode = nodesToProcess.Dequeue();

                    reductionRule.ApplyReductionRule(currentNode, graph);

                    foreach (var child in currentNode.ChildBranches.Select(x => x.DownStream).Cast<SchemaGraphNode>())
                    {
                        nodesToProcess.Enqueue(child);
                    }
                }
            }
        }

        protected override SchemaGraphNode CreateNewNode(ConnectivityGraphNode dependentNode)
        {
            return new SchemaGraphNode(dependentNode.Item);
        }

        protected override ISchemaGraph InstantiateNewGraph(IMultipleRootGraph<ConnectivityGraphNode> graph)
        {
            return new SchemaGraph();
        }

        public override IEnumerable<ISchemaGraph> CreateGraph(IMultipleRootGraph<ConnectivityGraphNode> graph)
        {
            List<ISchemaGraph> graphs = new List<ISchemaGraph>(graph.GetRoots().Count());

            foreach (var root in graph.GetRoots())
            {
                ISchemaGraph newGraph = InstantiateNewGraph(graph);

                CreateSubNetworkGraph(root, newGraph);

                ApplyReductionRules(newGraph);

                graphs.Add(newGraph);
            }

            return graphs;
        }

        private void CreateSubNetworkGraph(ConnectivityGraphNode root, ISchemaGraph newGraph)
        {
            long connectivityBreakerGid = connectivityBreakerFinder.FindConnectivityBreaker(root);

            Queue<ConnectivityGraphNode> nodesToProcess = new Queue<ConnectivityGraphNode>();
            nodesToProcess.Enqueue(root);

            while (nodesToProcess.Count > 0)
            {
                ConnectivityGraphNode currentNode = nodesToProcess.Dequeue();

                SchemaGraphNode newNode = GetNode(newGraph, currentNode);

                if (ShouldSkipNode(connectivityBreakerGid, currentNode))
                {
                    continue;
                }

                foreach (var childNode in currentNode.ChildBranches.Select(x => x.DownStream).Cast<ConnectivityGraphNode>())
                {
                    SchemaGraphNode newChildNode = GetNode(newGraph, childNode);

                    graphBranchManipulator.AddBranch(newNode, newChildNode);

                    nodesToProcess.Enqueue(childNode);
                }
            }

            newGraph.MarkInterConnectedBreaker(connectivityBreakerGid);
        }
    }
}
