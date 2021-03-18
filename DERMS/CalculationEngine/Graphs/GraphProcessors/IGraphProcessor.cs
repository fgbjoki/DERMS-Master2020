namespace CalculationEngine.Graphs.GraphProcessors
{
    public interface IGraphProcessor<GraphType>
    {
        bool AddGraph(GraphType graph);
    }
}
