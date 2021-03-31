using CalculationEngine.Graphs.GraphReductionRules;
using CalculationEngine.Model.Topology.Graph;
using System.Collections.Generic;

namespace CalculationEngine.Graphs
{
    public abstract class BaseDepedentGraphCreator<DependentGraphType, DependentGraphNodeType, DepedentUponGraphType, DepdententUponGraphNodeType> :
            IGraphCreator<DependentGraphType, DepedentUponGraphType>

        where DependentGraphNodeType : DMSTypeGraphNode
        where DepdententUponGraphNodeType : DMSTypeGraphNode
    {
        protected IEnumerable<GraphReductionRule<DependentGraphNodeType>> reductionRules;

        public BaseDepedentGraphCreator()
        {
            reductionRules = GetReductionRules();
        }

        protected abstract IEnumerable<GraphReductionRule<DependentGraphNodeType>> GetReductionRules();

        protected abstract DependentGraphNodeType CreateNewNode(DepdententUponGraphNodeType dependentNode);

        protected abstract DependentGraphType InstantiateNewGraph(DepedentUponGraphType graph);

        public abstract IEnumerable<DependentGraphType> CreateGraph(DepedentUponGraphType graph);
    }
}
