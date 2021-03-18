namespace CalculationEngine.Model.Topology.Graph
{
    public interface ISingleRootGraph<GraphNodeType> : IGraph<GraphNodeType>
        where GraphNodeType : DMSTypeGraphNode
    {
        GraphNodeType GetRoot();
    }
}
