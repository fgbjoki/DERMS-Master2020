using FieldSimulator.PowerSimulator.Model.Graph.Graphs.Nodes;
using FieldSimulator.PowerSimulator.Storage;

namespace FieldSimulator.PowerSimulator.Model.Graph.GraphTraverser
{
    public class TopologyNodeCalculationSimulator : ITopologyNodeCalculationSimulator
    {
        private TopologyGraphTraverser topologyGraphTraverser;

        public TopologyNodeCalculationSimulator()
        {
            topologyGraphTraverser = new TopologyGraphTraverser();
        }

        public void Simulate(TopologyGraphNode root, PowerGridSimulatorStorage powerGridSimulatorStorage, double simulationInterval)
        {
            topologyGraphTraverser.LoadRoot(root);
            TraverseGraph(powerGridSimulatorStorage, simulationInterval);
        }

        private void TraverseGraph(PowerGridSimulatorStorage powerGridSimulatorStorage, double simulationInterval)
        {
            foreach (var node in topologyGraphTraverser)
            {
                node.Calculate(powerGridSimulatorStorage, simulationInterval);
            }
        }
    }
}
