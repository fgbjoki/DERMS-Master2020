namespace CalculationEngine.Model.Topology.Graph.Schema
{
    public interface ISchemaGraph : ISingleRootGraph<SchemaGraphNode>
    {
        long GetInterConnectedBreakerGid();
        bool MarkInterConnectedBreaker(long breakerGid);
    }
}
