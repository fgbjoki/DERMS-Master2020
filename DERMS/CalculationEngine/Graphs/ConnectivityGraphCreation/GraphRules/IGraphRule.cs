using CalculationEngine.Model.Topology.Graph.Connectivity;

namespace CalculationEngine.Graphs.ConnectivityGraphCreation.GraphRules
{
    public interface IGraphRule
    {
        void ApplyRule(ConnectivityGraph graph);
    }
}
