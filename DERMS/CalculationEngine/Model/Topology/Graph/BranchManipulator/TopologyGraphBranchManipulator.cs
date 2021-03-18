using CalculationEngine.Model.Topology.Graph.Topology;

namespace CalculationEngine.Model.Topology.Graph
{
    public class TopologyGraphBranchManipulator : BaseGraphBranchManipulator<TopologyGraphNode>
    {
        protected override GraphBranch<GraphNode> CreateNewBranch(TopologyGraphNode parent, TopologyGraphNode child)
        {
            return new TopologyGraphBranch(parent, child);
        }
    }
}
