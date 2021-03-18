using System.Collections.Generic;
using System.Linq;
using CalculationEngine.Model.Topology.Transaction;

namespace CalculationEngine.Graphs.ConnectivityGraphCreation
{
    class ConductingEquipmentTraverser : ConnectivityObjectTraverser
    {
        protected override ConnectivityObject ExtractEntity(Terminal terminal)
        {
            return terminal.ConnectivityNode;
        }

        protected override List<Terminal> GetNonVisitedTerminals(ConnectivityNodeTraverseWrapper topologyObject)
        {
            List<Terminal> nonVisitedTerminal = new List<Terminal>(1);
            ConductingEquipment conductingEquipment = topologyObject.TopologyObject as ConductingEquipment;

            // Root element
            if (topologyObject.VisitedTerminal == null)
            {
                nonVisitedTerminal.Add(conductingEquipment.Terminals.First());
            }
            // Non root element
            else
            {
                List<Terminal> connectedTerminals = conductingEquipment.Terminals;

                if (connectedTerminals.Count == 2)
                {
                    nonVisitedTerminal.Add(connectedTerminals.First(x => x.GlobalId != topologyObject.VisitedTerminal.GlobalId));
                }
            }

            return nonVisitedTerminal;
        }
    }
}
