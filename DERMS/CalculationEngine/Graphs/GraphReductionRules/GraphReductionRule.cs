using CalculationEngine.Model.Topology.Graph;
using Common.AbstractModel;
using System.Collections.Generic;

namespace CalculationEngine.Graphs.GraphReductionRules
{
    public abstract class GraphReductionRule<GraphNodeType>
        where GraphNodeType : DMSTypeGraphNode
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
