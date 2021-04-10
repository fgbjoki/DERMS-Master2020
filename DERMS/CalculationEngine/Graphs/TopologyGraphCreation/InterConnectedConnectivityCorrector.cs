using CalculationEngine.Model.Topology.Graph;
using CalculationEngine.Model.Topology.Graph.Topology;
using Common.AbstractModel;
using System.Collections.Generic;
using System.Linq;

namespace CalculationEngine.Graphs.TopologyGraphCreation
{
    public class InterConnectedConnectivityCorrector
    {
        private TopologyBreakerGraphBranchManipulator breakerBranchManipulator;

        public InterConnectedConnectivityCorrector(TopologyBreakerGraphBranchManipulator breakerBranchManipulator)
        {
            this.breakerBranchManipulator = breakerBranchManipulator;
        }

        public void CorrectInterConnection(IMultipleRootGraph<TopologyGraphNode> graph)
        {
            TopologyGraphNode connectivityNode = TraverseToInterConnectedConnectivityNode(graph.GetRoots().First());
            if (connectivityNode == null)
            {
                return;
            }

            TopologyGraphNode interConnectedBreaker = GetBreaker(connectivityNode);
            TopologyGraphNode secondConnectivityNode = GetOtherConnectivityNode(interConnectedBreaker, connectivityNode.Item);

            breakerBranchManipulator.AddBranch(connectivityNode, secondConnectivityNode);
            breakerBranchManipulator.AddBranch(secondConnectivityNode, connectivityNode);

            RemoveBreakerBranches(interConnectedBreaker, connectivityNode, secondConnectivityNode);
        }

        private void RemoveBreakerBranches(TopologyGraphNode interConnectedBreaker, TopologyGraphNode firstConnectivityNode, TopologyGraphNode secondConnectivityNode)
        {
            foreach (var branch in interConnectedBreaker.ChildBranches.ToList())
            {
                breakerBranchManipulator.DeleteBranch(branch);
            }

            foreach (var branch in firstConnectivityNode.ChildBranches.ToList())
            {
                TopologyGraphNode child = branch.DownStream as TopologyGraphNode;
                if (child.Item == interConnectedBreaker.Item)
                {
                    breakerBranchManipulator.DeleteBranch(branch);
                }
            }

            foreach (var branch in secondConnectivityNode.ChildBranches.ToList())
            {
                TopologyGraphNode child = branch.DownStream as TopologyGraphNode;
                if (child.Item == interConnectedBreaker.Item)
                {
                    breakerBranchManipulator.DeleteBranch(branch);
                }
            }
        }

        private TopologyGraphNode GetOtherConnectivityNode(TopologyGraphNode interConnectedBreaker, long connectivityNodeGid)
        {
            foreach (var childBranch in interConnectedBreaker.ChildBranches.Select(x => x.DownStream).Cast<TopologyGraphNode>())
            {
                if (childBranch.Item != connectivityNodeGid)
                {
                    return childBranch;
                }
            }

            return null;
        }

        private TopologyGraphNode GetBreaker(TopologyGraphNode connectivityNode)
        {
            foreach (var child in connectivityNode.ChildBranches.Select(x => x.DownStream).Cast<TopologyGraphNode>())
            {
                if (child.DMSType == DMSType.BREAKER && IsNodeConnectedToItsParent(child, connectivityNode.Item))
                {
                    return child;
                }
            }

            return null;
        }

        private TopologyGraphNode TraverseToInterConnectedConnectivityNode(TopologyGraphNode root)
        {
            Queue<TopologyGraphNode> nodesToProcess = new Queue<TopologyGraphNode>();
            nodesToProcess.Enqueue(root);

            while (nodesToProcess.Count > 0)
            {
                TopologyGraphNode currentNode = nodesToProcess.Dequeue();

                if (currentNode.DMSType == DMSType.CONNECTIVITYNODE && IsConnectivityNodeConnectedToInterConnectedBreaker(currentNode))
                {
                    return currentNode;
                }

                foreach (var child in currentNode.ChildBranches.Select(x => x.DownStream).Cast<TopologyGraphNode>())
                {
                    nodesToProcess.Enqueue(child);
                }
            }

            return null;
        }

        private bool IsConnectivityNodeConnectedToInterConnectedBreaker(TopologyGraphNode connectivityNode)
        {
            foreach (var child in connectivityNode.ChildBranches.Select(x => x.DownStream).Cast<TopologyGraphNode>())
            {
                if (child.DMSType != DMSType.BREAKER)
                {
                    continue;
                }

                if (IsNodeConnectedToItsParent(child, connectivityNode.Item))
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsNodeConnectedToItsParent(TopologyGraphNode connectivityNode, long parentGid)
        {
            foreach (var child in connectivityNode.ChildBranches.Select(x => x.DownStream).Cast<TopologyGraphNode>())
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
