using Common.AbstractModel;
using FieldSimulator.PowerSimulator.Model.Graph.GraphManipulators;
using FieldSimulator.PowerSimulator.Model.Graph.Graphs.Branches;
using FieldSimulator.PowerSimulator.Model.Graph.Graphs.Nodes;
using System.Collections.Generic;
using System.Linq;

namespace FieldSimulator.PowerSimulator.Model.Graph.TopologyGraphCreator
{
    public class InterConnectedConnectivityCorrector
    {
        private TopologyBreakerGraphBranchManipulator breakerBranchManipulator;
        private List<TopologyBreakerGraphBranch> breakerBranches;

        public InterConnectedConnectivityCorrector(TopologyBreakerGraphBranchManipulator breakerBranchManipulator)
        {
            this.breakerBranchManipulator = breakerBranchManipulator;

            breakerBranches = new List<TopologyBreakerGraphBranch>();
        }

        public void CorrectInterConnection(IMultipleRootGraph<TopologyGraphNode> graph)
        {
            TopologyGraphNode connectivityNode = TraverseToInterConnectedConnectivityNode(graph.GetRoots().First());
            if (connectivityNode == null)
            {
                return;
            }

            TopologyGraphNode interConnectedBreaker = GetBreaker(connectivityNode);
            TopologyGraphNode secondConnectivityNode = GetOtherConnectivityNode(interConnectedBreaker, connectivityNode.GlobalId);

            TopologyBreakerGraphBranch breakerBranch = breakerBranchManipulator.AddBranch(connectivityNode, secondConnectivityNode) as TopologyBreakerGraphBranch;
            breakerBranch.BreakerGlobalId = interConnectedBreaker.GlobalId;
            breakerBranches.Add(breakerBranch);

            breakerBranch = breakerBranchManipulator.AddBranch(secondConnectivityNode, connectivityNode) as TopologyBreakerGraphBranch;
            breakerBranch.BreakerGlobalId = interConnectedBreaker.GlobalId;
            breakerBranches.Add(breakerBranch);

            RemoveBreakerBranches(interConnectedBreaker, connectivityNode, secondConnectivityNode);
            graph.RemoveNode(interConnectedBreaker.GlobalId);
        }

        public List<TopologyBreakerGraphBranch> GetBreakerBranches()
        {
            return breakerBranches;
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
                if (child.GlobalId == interConnectedBreaker.GlobalId)
                {
                    breakerBranchManipulator.DeleteBranch(branch);
                }
            }

            foreach (var branch in secondConnectivityNode.ChildBranches.ToList())
            {
                TopologyGraphNode child = branch.DownStream as TopologyGraphNode;
                if (child.GlobalId == interConnectedBreaker.GlobalId)
                {
                    breakerBranchManipulator.DeleteBranch(branch);
                }
            }
        }

        private TopologyGraphNode GetOtherConnectivityNode(TopologyGraphNode interConnectedBreaker, long connectivityNodeGid)
        {
            foreach (var childBranch in interConnectedBreaker.ChildBranches.Select(x => x.DownStream).Cast<TopologyGraphNode>())
            {
                if (childBranch.GlobalId != connectivityNodeGid)
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
                if (child.DMSType == DMSType.BREAKER && IsNodeConnectedToItsParent(child, connectivityNode.GlobalId))
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

                if (IsNodeConnectedToItsParent(child, connectivityNode.GlobalId))
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
                if (child.GlobalId == parentGid)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
