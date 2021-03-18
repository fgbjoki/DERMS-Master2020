namespace CalculationEngine.Model.Topology.Graph
{
    public class GraphBranch<T>
        where T : GraphNode
    {
        public GraphBranch(T parent, T child)
        {
            UpStream = parent;
            DownStream = child;
        }

        public T UpStream { get; private set; }

        public T DownStream { get; private set; }

        public void ReverseDirection()
        {
            T swapItem = UpStream;
            UpStream = DownStream;
            DownStream = swapItem;
        }

    }
}
