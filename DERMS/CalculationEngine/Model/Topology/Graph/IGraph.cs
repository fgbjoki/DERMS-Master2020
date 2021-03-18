using System.Collections.Generic;

namespace CalculationEngine.Model.Topology.Graph
{
    public interface IGraph<GraphNodeType> 
        where GraphNodeType : DMSTypeGraphNode
    {
        bool AddNode(GraphNodeType node);
        bool NodeExists(long globalId);
        IEnumerable<GraphNodeType> GetAllNodes();
        GraphNodeType GetNode(long globalId);
        bool RemoveNode(long globalId);
    }
}