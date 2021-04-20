using FieldSimulator.PowerSimulator.Calculations;
using System.Collections.Generic;
using FieldSimulator.PowerSimulator.Storage;

namespace FieldSimulator.PowerSimulator.Model.Graph.Graphs.Nodes
{
    public class TopologyGraphNode : GraphNode, ICalculationNode
    {
        public TopologyGraphNode(long globalId) : base(globalId)
        {
            Shunts = new List<Shunt>();
        }

        public List<Shunt> Shunts { get; private set; }

        public Calculation Calculation { get; set; }

        public void Calculate(PowerGridSimulatorStorage powerGridSimulatorStorage, double simulationInterval)
        {
            Calculation?.Calculate(powerGridSimulatorStorage, simulationInterval);

            foreach (var shunt in Shunts)
            {
                shunt?.Calculate(powerGridSimulatorStorage, simulationInterval);
            }
        }
    }
}
