using Common.Logger;
using System.Collections.Generic;
using System.Linq;

namespace CalculationEngine.Model.Topology.Graph
{
    public class Graph<GraphNodeType> : IGraph<GraphNodeType>
        where GraphNodeType : DMSTypeGraphNode
    {
        protected Dictionary<long, GraphNodeType> nodes;

        public Graph()
        {
            nodes = new Dictionary<long, GraphNodeType>();
        }

        public virtual bool NodeExists(long globalId)
        {
            return nodes.ContainsKey(globalId);
        }

        public virtual bool AddNode(GraphNodeType node)
        {
            if (nodes.ContainsKey(node.Item))
            {
                Logger.Instance.Log($"[{this.GetType()}] Node with gid: {node.Item:X16} already exists!");
                return false;
            }

            nodes.Add(node.Item, node);

            return true;
        }

        public virtual GraphNodeType GetNode(long globalId)
        {
            GraphNodeType node;

            nodes.TryGetValue(globalId, out node);

            return node;
        }

        public virtual IEnumerable<GraphNodeType> GetAllNodes()
        {
            List<GraphNodeType> allNodes = new List<GraphNodeType>(nodes.Count);

            foreach (var node in nodes.Select(x => x.Value))
            {
                allNodes.Add(node);
            }

            return allNodes;
        }

        public virtual bool RemoveNode(long globalId)
        {
            return nodes.Remove(globalId);
        }
    }
}
