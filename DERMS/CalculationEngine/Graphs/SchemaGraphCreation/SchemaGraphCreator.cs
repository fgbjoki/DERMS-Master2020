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
    public class SchemaGraphCreator : BaseDepedentGraphCreator<ISingleRootGraph<SchemaGraphNode>, SchemaGraphNode, IMultipleRootGraph<ConnectivityGraphNode>, ConnectivityGraphNode>
    {
        private SchemaGraphBranchManipulator graphBranchManipulator;
        private ConnectivityBreakerNodeFinder connectivityBreakerFinder;

        public SchemaGraphCreator()
        {
            connectivityBreakerFinder = new ConnectivityBreakerNodeFinder();
        }

        protected override IEnumerable<GraphReductionRule<SchemaGraphNode>> GetReductionRules()
        {
            graphBranchManipulator = new SchemaGraphBranchManipulator();

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

            foreach (var reductionRule in reductionRules)
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

        protected override ISingleRootGraph<SchemaGraphNode> InstantiateNewGraph(IMultipleRootGraph<ConnectivityGraphNode> graph)
        {
            return new SchemaGraph();
        }

        public override IEnumerable<ISingleRootGraph<SchemaGraphNode>> CreateGraph(IMultipleRootGraph<ConnectivityGraphNode> graph)
        {
            List<ISingleRootGraph<SchemaGraphNode>> graphs = new List<ISingleRootGraph<SchemaGraphNode>>(graph.GetRoots().Count());

            foreach (var root in graph.GetRoots())
            {
                ISingleRootGraph<SchemaGraphNode> newGraph = InstantiateNewGraph(graph);

                CreateSubNetworkGraph(root, newGraph);

                ApplyReductionRules(newGraph);
            }

            return graphs;
        }

        private void CreateSubNetworkGraph(ConnectivityGraphNode root, ISingleRootGraph<SchemaGraphNode> newGraph)
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
        }
    }
}
