using FieldSimulator.PowerSimulator.Model.Connectivity;
using FieldSimulator.PowerSimulator.Model.Equipment;
using System.Collections.Generic;
using System.Linq;

namespace FieldSimulator.PowerSimulator.Model.Graph.ConnectivityGraphCreator
{
    class ConductingEquipmentTraverser : ConnectivityObjectTraverser
    {
        protected override IdentifiedObject ExtractEntity(Terminal terminal)
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
