using FieldSimulator.PowerSimulator.Model.Graph.Graphs.Nodes;

namespace FieldSimulator.PowerSimulator.Model.Graph.Graphs.Branches
{
    public class TopologyGraphBranch : GraphBranch<GraphNode>
    {
        public TopologyGraphBranch(TopologyGraphNode parent, TopologyGraphNode child) : base(parent, child)
        {
        }

        public virtual bool DoesBranchConduct()
        {
            return true;
        }
    }
}
