namespace CalculationEngine.Model.Topology.Graph
{
    public class GenericGraphNode<T> : GraphNode
    {
        public GenericGraphNode(T item) : base()
        {
            Item = item;
        }

        public T Item { get; set; }
    }
}
