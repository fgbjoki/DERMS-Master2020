using System.Collections.Generic;
using CalculationEngine.Model.Topology.Graph;
using CalculationEngine.Model.Topology.Graph.Topology;

namespace CalculationEngine.Graphs.GraphReductionRules.Topology
{
    public class BreakerReductionGraphRule : TopologyACLSBranchGraphRule
    {
        private List<TopologyBreakerGraphBranch> breakerBranches;

        public BreakerReductionGraphRule(TopologyBreakerGraphBranchManipulator graphBranchManipulator) : base(graphBranchManipulator)
        {
            breakerBranches = new List<TopologyBreakerGraphBranch>();
        }

        protected override bool IsNeededNeighbour(TopologyGraphNode node)
        {
            return node.DMSType == Common.AbstractModel.DMSType.BREAKER;
        }

        protected override GraphBranch<GraphNode> AddNewBranch(TopologyGraphNode parent, TopologyGraphNode child, TopologyGraphNode branch)
        {
            TopologyBreakerGraphBranch breakerBranch = base.AddNewBranch(parent, child, branch) as TopologyBreakerGraphBranch;
            breakerBranch.BreakerGlobalId = branch.Item;

            breakerBranches.Add(breakerBranch);

            return breakerBranch;
        }

        public List<TopologyBreakerGraphBranch> GetBreakerBranches()
        {
            return breakerBranches;
        }
    }
}
