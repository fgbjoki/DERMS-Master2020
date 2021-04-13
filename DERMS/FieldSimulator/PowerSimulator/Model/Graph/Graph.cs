using Common.Logger;
using FieldSimulator.PowerSimulator.Model.Graph.Graphs.Nodes;
using System.Collections.Generic;
using System.Linq;

namespace FieldSimulator.PowerSimulator.Model.Graph
{
    public class Graph<GraphNodeType> : IGraph<GraphNodeType>
        where GraphNodeType : GraphNode
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
            if (nodes.ContainsKey(node.GlobalId))
            {
                Logger.Instance.Log($"[{this.GetType()}] Node with gid: {node.GlobalId:X16} already exists!");
                return false;
            }

            nodes.Add(node.GlobalId, node);

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
