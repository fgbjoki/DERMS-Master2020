using System.Collections.Generic;

namespace CalculationEngine.Graphs.ConnectivityGraphCreation
{
    public interface IConnectivityObjectTraverser
    {
        List<ConnectivityNodeTraverseWrapper> ExploreNeighbourObjects(ConnectivityNodeTraverseWrapper topologyObject);
    }
}
