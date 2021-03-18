using CalculationEngine.Model.Topology.Graph.Schema;

namespace CalculationEngine.Model.Topology.Graph
{
    public class SchemaGraphBranchManipulator : BaseGraphBranchManipulator<SchemaGraphNode>
    {
        protected override GraphBranch<GraphNode> CreateNewBranch(SchemaGraphNode parent, SchemaGraphNode child)
        {
            return new GraphBranch<GraphNode>(parent, child);
        }
    }
}
