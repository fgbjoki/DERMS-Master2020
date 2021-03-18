using CalculationEngine.Model.Topology.Transaction;

namespace CalculationEngine.Graphs.ConnectivityGraphCreation
{
    public struct ConnectivityNodeTraverseWrapper
    {
        public ConnectivityNodeTraverseWrapper(Terminal visitedTerminal, ConnectivityObject topologyObject)
        {
            VisitedTerminal = visitedTerminal;
            TopologyObject = topologyObject;
        }

        public Terminal VisitedTerminal { get; set; }
        public ConnectivityObject TopologyObject { get; set; }
    }
}
