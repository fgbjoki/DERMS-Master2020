using CalculationEngine.Model.Topology.Graph.Schema;
using CalculationEngine.Model.Topology.Graph;

namespace CalculationEngine.Graphs.GraphReductionRules.Schema
{
    public class SchemaACLSBranchGraphRule : ReduceACLineSegmentBranchesGraphRule<SchemaGraphNode>
    {
        public SchemaACLSBranchGraphRule(BaseGraphBranchManipulator<SchemaGraphNode> graphBranchManipulator) : base(graphBranchManipulator)
        {
        }
    }
}
