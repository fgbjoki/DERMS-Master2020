using System.Collections.Generic;

namespace FieldSimulator.PowerSimulator.Model.Graph.ConnectivityGraphCreator
{
    public interface IConnectivityObjectTraverser
    {
        List<ConnectivityNodeTraverseWrapper> ExploreNeighbourObjects(ConnectivityNodeTraverseWrapper topologyObject);
    }
}
