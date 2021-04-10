using CalculationEngine.Model.Topology.Graph;
using System.Collections.Generic;
using System.Linq;

namespace CalculationEngine.Graphs
{
    public abstract class MultipleRootDependentGraphCreator<NewGraphNodeType, DependentUponNodeType> :
            BaseDepedentGraphCreator<IMultipleRootGraph<NewGraphNodeType>, NewGraphNodeType, IMultipleRootGraph<DependentUponNodeType>, DependentUponNodeType>
        where NewGraphNodeType : DMSTypeGraphNode
        where DependentUponNodeType : DMSTypeGraphNode
    {
        private BaseGraphBranchManipulator<NewGraphNodeType> graphBranchManipulator;

        public MultipleRootDependentGraphCreator(BaseGraphBranchManipulator<NewGraphNodeType> graphBranchManipulator) : base()
        {
            this.graphBranchManipulator = graphBranchManipulator;
        }

        public override IEnumerable<IMultipleRootGraph<NewGraphNodeType>> CreateGraph(IMultipleRootGraph<DependentUponNodeType> graph)
        {
            List<IMultipleRootGraph<NewGraphNodeType>> newGraphs = new List<IMultipleRootGraph<NewGraphNodeType>>(1);

            IMultipleRootGraph<NewGraphNodeType> newGraph = InstantiateNewGraph(graph);

            foreach (var node in graph.GetAllNodes())
            {
                NewGraphNodeType newNode = GetNode(newGraph, node);

                foreach (var childNode in node.ChildBranches.Select(x => x.DownStream).Cast<DependentUponNodeType>())
                {
                    NewGraphNodeType newChildNode = GetNode(newGraph, childNode);

                    graphBranchManipulator.AddBranch(newNode, newChildNode);
                }
            }

            ApplyReductionRules(newGraph);

            AdditionalProcessing(newGraph);

            newGraphs.Add(newGraph);

            return newGraphs;
        }

        protected virtual void AdditionalProcessing(IMultipleRootGraph<NewGraphNodeType> graph)
        {

        }

        private NewGraphNodeType GetNode(IMultipleRootGraph<NewGraphNodeType> graph, DependentUponNodeType oldNode)
        {
            NewGraphNodeType node = graph.GetNode(oldNode.Item);

            if (node == null)
            {
                node = CreateNewNode(oldNode);
                graph.AddNode(node);
            }

            return node;
        }

        private void ApplyReductionRules(IMultipleRootGraph<NewGraphNodeType> graph)
        {
            foreach (var root in graph.GetRoots())
            {
                Queue<NewGraphNodeType> nodesToProcess = new Queue<NewGraphNodeType>();
                nodesToProcess.Enqueue(root);

                while (nodesToProcess.Count > 0)
                {
                    NewGraphNodeType currentNode = nodesToProcess.Dequeue();

                    foreach (var reductionRule in GetReductionRules())
                    {
                        reductionRule.ApplyReductionRule(currentNode, graph);
                    }

                    foreach (var child in currentNode.ChildBranches.Select(x => x.DownStream).Cast<NewGraphNodeType>())
                    {
                        if (!IsNodeConnectedToParent(child, currentNode.Item))
                        {
                            continue;
                        }

                        nodesToProcess.Enqueue(child);
                    }
                }
            }
        }

        private bool IsNodeConnectedToParent(NewGraphNodeType node, long parentGid)
        {
            foreach (var child in node.ChildBranches.Select(x => x.DownStream).Cast<NewGraphNodeType>())
            {
                if (child.Item == parentGid)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
