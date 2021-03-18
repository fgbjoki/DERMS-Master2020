namespace CalculationEngine.Model.Topology.Graph
{
    public class GraphBranchManipulator : BaseGraphBranchManipulator<GraphNode>
    {
        protected override GraphBranch<GraphNode> CreateNewBranch(GraphNode parent, GraphNode child)
        {
            return new GraphBranch<GraphNode>(parent, child);
        }
    }
}
