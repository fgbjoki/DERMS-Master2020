using CalculationEngine.Model.Topology.Graph.Topology;

namespace CalculationEngine.Model.Topology.Graph
{
    public class TopologyBreakerGraphBranchManipulator : TopologyGraphBranchManipulator
    {
        protected override GraphBranch<GraphNode> CreateNewBranch(TopologyGraphNode parent, TopologyGraphNode child)
        {
            return new TopologyBreakerGraphBranch(parent, child);
        }
    }
}
