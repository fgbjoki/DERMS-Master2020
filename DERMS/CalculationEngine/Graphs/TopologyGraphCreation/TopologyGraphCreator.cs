using CalculationEngine.Model.Topology.Graph.Connectivity;
using CalculationEngine.Model.Topology.Graph.Topology;
using System.Collections.Generic;
using CalculationEngine.Model.Topology.Graph;
using CalculationEngine.Graphs.GraphReductionRules;
using CalculationEngine.Graphs.GraphReductionRules.Topology;

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

        public IEnumerable<TopologyBreakerGraphBranch> GetBreakerBranches()
        {
            return breakerReductionGraphRule.GetBreakerBranches();
        }
    }
}
