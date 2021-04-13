using FieldSimulator.PowerSimulator.Model.Connectivity;

namespace FieldSimulator.PowerSimulator.Model.Graph.ConnectivityGraphCreator
{
    public struct ConnectivityNodeTraverseWrapper
    {
        public ConnectivityNodeTraverseWrapper(Terminal visitedTerminal, IdentifiedObject topologyObject)
        {
            VisitedTerminal = visitedTerminal;
            TopologyObject = topologyObject;
        }

        public Terminal VisitedTerminal { get; set; }
        public IdentifiedObject TopologyObject { get; set; }
    }
}
