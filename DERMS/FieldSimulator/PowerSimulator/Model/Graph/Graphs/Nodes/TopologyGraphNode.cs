using FieldSimulator.PowerSimulator.Model.Equipment;
using System.Collections.Generic;

namespace FieldSimulator.PowerSimulator.Model.Graph.Graphs.Nodes
{
    public class TopologyGraphNode : GraphNode
    {
        public TopologyGraphNode(long globalId) : base(globalId)
        {
            Shunts = new List<Shunt>();
        }

        public List<Shunt> Shunts { get; private set; }

        public ConductingEquipment ConductingEquipment { get; set; }
    }
}
