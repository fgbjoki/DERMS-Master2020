using System.Linq;
using FieldSimulator.PowerSimulator.Model.Graph.GraphManipulators;
using FieldSimulator.PowerSimulator.Model.Graph.Graphs;
using FieldSimulator.PowerSimulator.Model.Graph.Graphs.Nodes;
using FieldSimulator.PowerSimulator.Model.Graph.Graphs.Branches;

namespace FieldSimulator.PowerSimulator.Model.Graph.ConnectivityGraphCreator.GraphRules
{
    public class ReversePathToInterConnectedBreakerGraphRule : IGraphRule
    {
        private GraphBranchManipulator branchManipulator;

        public ReversePathToInterConnectedBreakerGraphRule(GraphBranchManipulator branchManipulator)
        {
            this.branchManipulator = branchManipulator;
        }

        public void ApplyRule(ConnectivityGraph graph)
        {
            foreach (var root in graph.GetRoots())
            {
                ConnectivityGraphNode aclineSegmentNode;
                if (IsEnergySourceConnectedToInterConnectedBreaker(root, out aclineSegmentNode))
                {
                    continue;
                }

                ReverseBranchAclineSegment(aclineSegmentNode);

                AddNewBranches(aclineSegmentNode.ChildBranches.First().DownStream as ConnectivityGraphNode);
            }
        }

        private void AddNewBranches(ConnectivityGraphNode connectivityNode)
        {
            // 1. Breaker, 2.ConnectivityNode
            int numberOfNodesToProcess = 2;

            ConnectivityGraphNode currentNode = connectivityNode;

            for (int i = 0; i < numberOfNodesToProcess; i++)
            {
                currentNode = AddNewBranchToFirstParent(currentNode);
            }
        }

        private ConnectivityGraphNode AddNewBranchToFirstParent(ConnectivityGraphNode node)
        {
            ConnectivityGraphNode parent = node.ParentBranches.First().UpStream as ConnectivityGraphNode;
            branchManipulator.AddBranch(node, parent);

            return parent;
        }

        private bool IsEnergySourceConnectedToInterConnectedBreaker(ConnectivityGraphNode root, out ConnectivityGraphNode aclineSegmentNode)
        {
            aclineSegmentNode = null;

            ConnectivityGraphNode aclineSegment = GetFirstACLineSegment(root);

            if (aclineSegment.ChildBranches.Count != 0)
            {
                return true;
            }

            aclineSegmentNode = aclineSegment;

            return false;
        }

        private ConnectivityGraphNode GetFirstACLineSegment(ConnectivityGraphNode root)
        {
            ConnectivityGraphNode currentNode = root;

            while (currentNode.DMSType != Common.AbstractModel.DMSType.ACLINESEG)
            {
                currentNode = currentNode.ChildBranches.First().DownStream as ConnectivityGraphNode;
            }

            return currentNode;
        }

        private void ReverseBranchAclineSegment(ConnectivityGraphNode node)
        {
            GraphBranch<GraphNode> branch = node.ParentBranches.First();

            branchManipulator.ReverseBranchDirection(branch);
        }
    }
}
