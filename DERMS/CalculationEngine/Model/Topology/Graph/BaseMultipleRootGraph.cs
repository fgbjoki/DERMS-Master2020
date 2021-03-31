using CalculationEngine.Model.Topology.Graph;
using Common.AbstractModel;
using System.Collections.Generic;
using System.Linq;

namespace CalculationEngine.Graphs
{
    public abstract class BaseMultipleRootGraph<GraphNodeType> : Graph<GraphNodeType>, IMultipleRootGraph<GraphNodeType>
        where GraphNodeType : DMSTypeGraphNode
    {
        private List<GraphNodeType> roots;

        protected BaseMultipleRootGraph() : base()
        {
            roots = new List<GraphNodeType>();
        }

        public override bool NodeExists(long globalId)
        {
            if (base.NodeExists(globalId))
            {
                return true;
            }

            return roots.Exists(x => x.Item == globalId);
        }

        public override bool AddNode(GraphNodeType node)
        {
            if (node.DMSType == DMSType.ENERGYSOURCE)
            {
                if (roots.Exists(x => x.Item == node.Item))
                {
                    return false;
                }

                roots.Add(node);
                return true;
            }
            else
            {
                return base.AddNode(node);
            }
        }

        public override GraphNodeType GetNode(long globalId)
        {
            GraphNodeType node = roots.FirstOrDefault(x => x.Item == globalId);

            if (node == null)
            {
                node = base.GetNode(globalId);
            }

            return node;
        }

        public override IEnumerable<GraphNodeType> GetAllNodes()
        {
            List<GraphNodeType> allNodes = new List<GraphNodeType>(roots);

            allNodes.Concat(base.GetAllNodes());

            return allNodes;
        }

        public IEnumerable<GraphNodeType> GetRoots()
        {
            return new List<GraphNodeType>(roots);
        }

        public override bool RemoveNode(long globalId)
        {
            GraphNodeType node = roots.FirstOrDefault(x => x.Item == globalId);

            if (node != null)
            {
                return roots.Remove(node);
            }

            return base.RemoveNode(globalId);
        }
    }
}
