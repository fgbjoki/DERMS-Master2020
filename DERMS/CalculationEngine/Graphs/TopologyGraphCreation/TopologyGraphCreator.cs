using CalculationEngine.Model.Topology.Graph.Connectivity;
using CalculationEngine.Model.Topology.Graph.Topology;
using System.Collections.Generic;
using CalculationEngine.Model.Topology.Graph;
using CalculationEngine.Graphs.GraphReductionRules;
using CalculationEngine.Graphs.GraphReductionRules.Topology;
using System.Linq;

namespace CalculationEngine.Graphs.TopologyGraphCreation
{
    public class TopologyGraphCreator : MultipleRootDependentGraphCreator<TopologyGraphNode, ConnectivityGraphNode>
    {
        private TopologyGraphBranchManipulator aclineSegmentBranchManipulator;
        private TopologyBreakerGraphBranchManipulator breakerBrachManipulator;

        private BreakerReductionGraphRule breakerReductionGraphRule;

        public TopologyGraphCreator(TopologyGraphBranchManipulator aclineSegmentBranchManipulator, TopologyBreakerGraphBranchManipulator breakerBrachManipulator) : base(aclineSegmentBranchManipulator)
        {
            this.aclineSegmentBranchManipulator = aclineSegmentBranchManipulator;
            this.breakerBrachManipulator = breakerBrachManipulator;          
        }

        public IEnumerable<TopologyBreakerGraphBranch> GetBreakerBranches()
        {
            return breakerReductionGraphRule.GetBreakerBranches();
        }

        public override IEnumerable<IMultipleRootGraph<TopologyGraphNode>> CreateGraph(IMultipleRootGraph<ConnectivityGraphNode> graph)
        {
            IEnumerable<IMultipleRootGraph<TopologyGraphNode>> newGraphs = base.CreateGraph(graph);
            TopologyGraph newGraph = newGraphs.First() as TopologyGraph;

            newGraph.LoadBreakerBranches(GetBreakerBranches());

            return newGraphs;
        }

        protected override TopologyGraphNode CreateNewNode(ConnectivityGraphNode dependentNode)
        {
            return new TopologyGraphNode(dependentNode.Item);
        }

        protected override IEnumerable<GraphReductionRule<TopologyGraphNode>> GetReductionRules()
        {
            breakerReductionGraphRule = new BreakerReductionGraphRule(breakerBrachManipulator);

            return new List<GraphReductionRule<TopologyGraphNode>>()
            {
                new TopologyACLSBranchGraphRule(aclineSegmentBranchManipulator),
                breakerReductionGraphRule,
                new ConnectivityNodeShuntReductionGraphRule(aclineSegmentBranchManipulator)
            };
        }

        protected override IMultipleRootGraph<TopologyGraphNode> InstantiateNewGraph(IMultipleRootGraph<ConnectivityGraphNode> graph)
        {
            return new TopologyGraph();
        }
    }
}
