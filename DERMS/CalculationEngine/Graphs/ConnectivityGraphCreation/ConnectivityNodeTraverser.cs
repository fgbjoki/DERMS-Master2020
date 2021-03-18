using System.Collections.Generic;
using System.Linq;
using CalculationEngine.Model.Topology.Transaction;

namespace CalculationEngine.Graphs.ConnectivityGraphCreation
{
    public class ConnectivityNodeTraverser : ConnectivityObjectTraverser
    {
        protected override ConnectivityObject ExtractEntity(Terminal terminal)
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
