using FieldSimulator.PowerSimulator.Model.Connectivity;
using System.Collections.Generic;

namespace FieldSimulator.PowerSimulator.Model.Graph.ConnectivityGraphCreator
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
                IdentifiedObject neighbour = ExtractEntity(terminal);
                schemaObjects.Add(new ConnectivityNodeTraverseWrapper(terminal, neighbour));
            }

            return schemaObjects;
        }

        protected abstract List<Terminal> GetNonVisitedTerminals(ConnectivityNodeTraverseWrapper topologyObject);

        protected abstract IdentifiedObject ExtractEntity(Terminal terminal);
    }
}
