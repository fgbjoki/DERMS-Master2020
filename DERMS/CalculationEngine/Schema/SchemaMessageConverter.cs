using CalculationEngine.Model.Topology.Graph.Schema;
using Common.PubSub.Messages;
using System.Collections.Generic;
using System.Linq;

namespace CalculationEngine.Schema
{
    public class SchemaMessageConverter
    {
        public SchemaGraphChanged Convert(ISchemaGraph graph)
        {
            Dictionary<long, List<long>> parentToChildRelation = new Dictionary<long, List<long>>();

            SchemaGraphNode root = graph.GetRoot();

            Queue<SchemaGraphNode> nodesToProcess = new Queue<SchemaGraphNode>();
            nodesToProcess.Enqueue(root);

            while (nodesToProcess.Count > 0)
            {
                SchemaGraphNode currentNode = nodesToProcess.Dequeue();

                List<long> children = new List<long>(currentNode.ChildBranches.Count);

                foreach (var child in currentNode.ChildBranches.Select(x => x.DownStream).Cast<SchemaGraphNode>())
                {
                    children.Add(child.Item);
                }

                parentToChildRelation.Add(currentNode.Item, children);
            }

            return new SchemaGraphChanged(parentToChildRelation, graph.GetInterConnectedBreakerGid());
        }
    }
}
