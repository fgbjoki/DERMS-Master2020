using FieldSimulator.PowerSimulator.Model.Graph.GraphManipulators;
using FieldSimulator.PowerSimulator.Model.Graph.Graphs.Nodes;

namespace FieldSimulator.PowerSimulator.Model.Graph.TopologyGraphCreator.GraphRules
{
    public class TopologyACLSBranchGraphRule : ReduceACLineSegmentBranchesGraphRule<TopologyGraphNode>
    {
        public TopologyACLSBranchGraphRule(TopologyGraphBranchManipulator graphBranchManipulator) : base(graphBranchManipulator)
        {
        }
    }
}
