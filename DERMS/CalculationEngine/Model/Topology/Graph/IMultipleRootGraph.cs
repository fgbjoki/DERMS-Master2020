using System.Collections.Generic;

namespace CalculationEngine.Model.Topology.Graph
{
    public interface IMultipleRootGraph<GraphNodeType> : IGraph<GraphNodeType>
        where GraphNodeType : DMSTypeGraphNode
    {
        IEnumerable<GraphNodeType> GetRoots();

    }
}
