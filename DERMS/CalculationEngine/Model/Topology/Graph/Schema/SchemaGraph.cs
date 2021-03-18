using System.Collections.Generic;
using System.Linq;

namespace CalculationEngine.Model.Topology.Graph.Schema
{
    public class SchemaGraph : Graph<SchemaGraphNode>, ISingleRootGraph<SchemaGraphNode>
    {
        private SchemaGraphNode root;

        public override bool AddNode(SchemaGraphNode node)
        {
            if (node.DMSType == Common.AbstractModel.DMSType.ENERGYSOURCE)
            {
                if (root != null)
                {
                    return false;
                }

                root = node;
            }
            else
            {
                return base.AddNode(node);
            }

            return true;
        }

        public override bool NodeExists(long globalId)
        {
            if (root.Item == globalId)
            {
                return true;
            }
            else
            {
                return base.NodeExists(globalId);
            }
        }

        public override IEnumerable<SchemaGraphNode> GetAllNodes()
        {
            return base.GetAllNodes().Append(root);
        }

        public override SchemaGraphNode GetNode(long globalId)
        {
            if (root?.Item == globalId)
            {
                return root;
            }
            else
            {
                return base.GetNode(globalId);
            }
        }

        public override bool RemoveNode(long globalId)
        {
            if (root.Item == globalId)
            {
                root = null;
                return true;
            }

            return base.RemoveNode(globalId);
        }

        public SchemaGraphNode GetRoot()
        {
            return root;
        }
    }
}
