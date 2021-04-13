using Common.AbstractModel;
using FieldSimulator.PowerSimulator.Model.Graph.Graphs.Nodes;
using System.Collections.Generic;

namespace FieldSimulator.PowerSimulator.Model.Graph.TopologyGraphCreator.GraphRules
{
    public abstract class GraphReductionRule<GraphNodeType>
        where GraphNodeType : GraphNode
    {
        public GraphReductionRule(IEnumerable<DMSType> belongingTypes)
        {
            BelongingType = new List<DMSType>(belongingTypes);
        }

        public ICollection<DMSType> BelongingType { get; private set; }

        public void ApplyReductionRule(GraphNodeType node, IGraph<GraphNodeType> graph)
        {
            if (!IsNodeValid(node))
            {
                return;
            }

            ApplyRule(node, graph);
        }

        protected abstract void ApplyRule(GraphNodeType node, IGraph<GraphNodeType> graph);

        protected virtual bool IsNodeValid(GraphNodeType node)
        {
            return BelongingType.Contains(node.DMSType);
        }
    }
}
