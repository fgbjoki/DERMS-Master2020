using FieldSimulator.PowerSimulator.Model.Graph.Graphs.Branches;
using FieldSimulator.PowerSimulator.Model.Graph.Graphs.Nodes;

namespace FieldSimulator.PowerSimulator.Model.Graph.GraphManipulators
{
    public class TopologyBreakerGraphBranchManipulator : TopologyGraphBranchManipulator
    {
        protected override GraphBranch<GraphNode> CreateNewBranch(TopologyGraphNode parent, TopologyGraphNode child)
        {
            return new TopologyBreakerGraphBranch(parent, child);
        }
    }
}
