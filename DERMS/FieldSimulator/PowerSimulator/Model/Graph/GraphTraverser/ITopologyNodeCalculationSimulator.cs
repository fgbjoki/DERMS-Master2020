using FieldSimulator.PowerSimulator.Model.Graph.Graphs.Nodes;
using FieldSimulator.PowerSimulator.Storage;

namespace FieldSimulator.PowerSimulator.Model.Graph.GraphTraverser
{
    public interface ITopologyNodeCalculationSimulator
    {
        void Simulate(TopologyGraphNode root, PowerGridSimulatorStorage powerGridSimulatorStorage, double simulationInterval);
    }
}
