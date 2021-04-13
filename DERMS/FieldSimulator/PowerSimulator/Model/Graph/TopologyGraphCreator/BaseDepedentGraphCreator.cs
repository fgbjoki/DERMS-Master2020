using FieldSimulator.PowerSimulator.Model.Graph.Graphs.Nodes;
using FieldSimulator.PowerSimulator.Model.Graph.TopologyGraphCreator.GraphRules;
using System.Collections.Generic;

namespace FieldSimulator.PowerSimulator.Model.Graph.TopologyGraphCreator
{
    public abstract class BaseDepedentGraphCreator<DependentGraphType, DependentGraphNodeType, DepedentUponGraphType, DepdententUponGraphNodeType> :
            IGraphCreator<DependentGraphType, DepedentUponGraphType>

        where DependentGraphNodeType : GraphNode
        where DepdententUponGraphNodeType : GraphNode
    {
        public BaseDepedentGraphCreator()
        {
        }

        protected abstract IEnumerable<GraphReductionRule<DependentGraphNodeType>> GetReductionRules();

        protected abstract DependentGraphNodeType CreateNewNode(DepdententUponGraphNodeType dependentNode);

        protected abstract DependentGraphType InstantiateNewGraph(DepedentUponGraphType graph);

        public abstract IEnumerable<DependentGraphType> CreateGraph(DepedentUponGraphType graph);
    }
}
