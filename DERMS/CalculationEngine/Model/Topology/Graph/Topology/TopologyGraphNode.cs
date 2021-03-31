using System.Collections.Generic;

namespace CalculationEngine.Model.Topology.Graph.Topology
{
    public class TopologyGraphNode : DMSTypeGraphNode
    {
        public TopologyGraphNode(long globalId) : base(globalId)
        {
            Shunts = new List<Shunt>();
        }

        public List<Shunt> Shunts { get; private set; }
    }
}
