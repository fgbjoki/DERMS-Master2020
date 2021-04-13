using System.Collections.Generic;
using System.Linq;
using FieldSimulator.PowerSimulator.Model.Connectivity;

namespace FieldSimulator.PowerSimulator.Model.Graph.ConnectivityGraphCreator
{
    public class ConnectivityNodeTraverser : ConnectivityObjectTraverser
    {
        protected override IdentifiedObject ExtractEntity(Terminal terminal)
        {
            return terminal.ConductingEquipment;
        }

        protected override List<Terminal> GetNonVisitedTerminals(ConnectivityNodeTraverseWrapper topologyObject)
        {
            ConnectivityNode connectivityNode = topologyObject.TopologyObject as ConnectivityNode;

            return new List<Terminal>(connectivityNode.Terminals.Where(x => topologyObject.VisitedTerminal.GlobalId != x.GlobalId));
        }
    }
}
