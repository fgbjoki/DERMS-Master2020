namespace CalculationEngine.Model.Topology.Graph.Topology
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
