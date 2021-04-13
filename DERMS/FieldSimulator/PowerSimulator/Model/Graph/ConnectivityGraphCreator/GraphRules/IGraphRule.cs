using FieldSimulator.PowerSimulator.Model.Graph.Graphs;

namespace FieldSimulator.PowerSimulator.Model.Graph.ConnectivityGraphCreator.GraphRules
{
    public interface IGraphRule
    {
        void ApplyRule(ConnectivityGraph graph);
    }
}
