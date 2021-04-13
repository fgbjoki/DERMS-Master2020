using System.Linq;
using FieldSimulator.PowerSimulator.Model.Graph.GraphManipulators;
using FieldSimulator.PowerSimulator.Model.Graph.Graphs;
using FieldSimulator.PowerSimulator.Model.Graph.Graphs.Nodes;
using FieldSimulator.PowerSimulator.Model.Graph.Graphs.Branches;

namespace FieldSimulator.PowerSimulator.Model.Graph.ConnectivityGraphCreator.GraphRules
{
    public class EnergySourceDirectionReverseGraphRule : IGraphRule
    {
        private GraphBranchManipulator branchManipulator;

        public EnergySourceDirectionReverseGraphRule(GraphBranchManipulator branchManipulator)
        {
            this.branchManipulator = branchManipulator;
        }

        public void ApplyRule(ConnectivityGraph graph)
        {
            foreach (var root in graph.GetRoots())
            {
                if (root.ChildBranches.Count == 1 && root.ParentBranches.Count == 0)
                {
                    continue;
                }

                ReverseBranchDirections(root);
            }
        }

        private void ReverseBranchDirections(ConnectivityGraphNode root)
        {
            ConnectivityGraphNode currentNode = root;

            while (currentNode.DMSType != Common.AbstractModel.DMSType.ACLINESEG)
            {
                ReverseBranch(currentNode);

                currentNode = currentNode.ChildBranches.First().DownStream as ConnectivityGraphNode;
            }
        }

        private void ReverseBranch(ConnectivityGraphNode node)
        {
            GraphBranch<GraphNode> branch = node.ParentBranches.First();

            branchManipulator.ReverseBranchDirection(branch);
        }
    }
}
