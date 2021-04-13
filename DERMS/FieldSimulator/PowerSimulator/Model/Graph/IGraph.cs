using FieldSimulator.PowerSimulator.Model.Graph.Graphs.Nodes;
using System.Collections.Generic;

namespace FieldSimulator.PowerSimulator.Model.Graph
{
    public interface IGraph<GraphNodeType> 
        where GraphNodeType : GraphNode
    {
        bool AddNode(GraphNodeType node);
        bool NodeExists(long globalId);
        IEnumerable<GraphNodeType> GetAllNodes();
        GraphNodeType GetNode(long globalId);
        bool RemoveNode(long globalId);
    }
}