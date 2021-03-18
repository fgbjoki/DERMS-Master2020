using CalculationEngine.Model.Topology.Transaction;
using System.Collections.Generic;
using System.Linq;

namespace CalculationEngine.Graphs.ConnectivityGraphCreation
{
    public abstract class ConnectivityObjectTraverser : IConnectivityObjectTraverser
    {
        public List<ConnectivityNodeTraverseWrapper> ExploreNeighbourObjects(ConnectivityNodeTraverseWrapper topologyObject)
        {
            List<Terminal> nonVisitedTerminals = GetNonVisitedTerminals(topologyObject);

            if (nonVisitedTerminals.Count == 0)
            {
                return new List<ConnectivityNodeTraverseWrapper>(0);
            }

            List<ConnectivityNodeTraverseWrapper> schemaObjects = new List<ConnectivityNodeTraverseWrapper>(nonVisitedTerminals.Count);

            foreach (Terminal terminal in nonVisitedTerminals)
            {
                ConnectivityObject neighbour = ExtractEntity(terminal);
                schemaObjects.Add(new ConnectivityNodeTraverseWrapper(terminal, neighbour));
            }

            return schemaObjects;
        }

        protected abstract List<Terminal> GetNonVisitedTerminals(ConnectivityNodeTraverseWrapper topologyObject);

        protected abstract ConnectivityObject ExtractEntity(Terminal terminal);
    }
}
