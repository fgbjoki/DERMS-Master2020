using CalculationEngine.Model.Topology.Graph.Topology;
using CalculationEngine.Model.Topology.Graph;

namespace CalculationEngine.Graphs.GraphReductionRules.Topology
{
    public class TopologyACLSBranchGraphRule : ReduceACLineSegmentBranchesGraphRule<TopologyGraphNode>
    {
        public TopologyACLSBranchGraphRule(TopologyGraphBranchManipulator graphBranchManipulator) : base(graphBranchManipulator)
        {
        }
    }
}
