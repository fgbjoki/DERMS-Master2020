using FieldSimulator.PowerSimulator.Model.Graph.Graphs.Nodes;
using System.Collections.Generic;

namespace FieldSimulator.PowerSimulator.Model.Graph
{
    public interface IMultipleRootGraph<GraphNodeType> : IGraph<GraphNodeType>
        where GraphNodeType : GraphNode
    {
        IEnumerable<GraphNodeType> GetRoots();
    }
}
