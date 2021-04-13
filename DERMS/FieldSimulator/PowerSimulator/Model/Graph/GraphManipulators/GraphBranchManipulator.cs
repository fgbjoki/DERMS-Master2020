using FieldSimulator.PowerSimulator.Model.Graph.Graphs.Branches;
using FieldSimulator.PowerSimulator.Model.Graph.Graphs.Nodes;

namespace FieldSimulator.PowerSimulator.Model.Graph.GraphManipulators
{
    public class GraphBranchManipulator : BaseGraphBranchManipulator<GraphNode>
    {
        protected override GraphBranch<GraphNode> CreateNewBranch(GraphNode parent, GraphNode child)
        {
            return new GraphBranch<GraphNode>(parent, child);
        }
    }
}
