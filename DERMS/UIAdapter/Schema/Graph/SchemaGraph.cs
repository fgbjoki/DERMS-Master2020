using System.Collections.Generic;
using System.Linq;

namespace UIAdapter.Schema.Graph
{
    public class SchemaGraph
    {
        private Dictionary<long, SchemaBreakerGraphNode> breakers;
        private Dictionary<long, SchemaGraphNode> nodes;

        private long root;
        private long interConnectedBreaker;

        public SchemaGraph()
        {
            breakers = new Dictionary<long, SchemaBreakerGraphNode>();
            nodes = new Dictionary<long, SchemaGraphNode>();
        }

        public void AddBreaker(SchemaBreakerGraphNode breakerNode)
        {
            breakers.Add(breakerNode.GlobalId, breakerNode);
            nodes.Add(breakerNode.GlobalId, breakerNode);
        }

        public void MarkRoot(long root)
        {
            this.root = root;
        }

        public void MarkInterConnectedBreaker(long interConnectedBreaker)
        {
            this.interConnectedBreaker = interConnectedBreaker;
        }

        public void RemoveNode(long globalId)
        {
            nodes.Remove(globalId);
        }

        public SchemaGraphNode GetRoot()
        {
            SchemaGraphNode root;

            nodes.TryGetValue(this.root, out root);

            return root;
        }

        public SchemaGraphNode GetInterConnectedBreaker()
        {
            SchemaGraphNode interConnectedBreaker;

            nodes.TryGetValue(this.interConnectedBreaker, out interConnectedBreaker);

            return interConnectedBreaker;
        }

        public SchemaGraphNode GetNode(long globalId)
        {
            if (root == globalId)
            {
                return GetRoot();
            }
            else if (interConnectedBreaker == globalId)
            {
                return GetInterConnectedBreaker();
            }
            else
            {
                SchemaGraphNode node;
                nodes.TryGetValue(globalId, out node);

                return node;
            }
        }

        public List<SchemaBreakerGraphNode> GetBreakers()
        {
            return breakers.Values.ToList();
        }

        public void AddNode(SchemaGraphNode node)
        {
            nodes.Add(node.GlobalId, node);
        }
    }
}
