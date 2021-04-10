using System.Collections.Generic;
using CalculationEngine.Model.Topology.Graph;
using CalculationEngine.Model.Topology.Graph.Topology;
using System.Linq;
using Common.AbstractModel;

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
            return node.DMSType == DMSType.BREAKER && !IsInterConnectedBreaker(node);
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

        protected bool IsInterConnectedBreaker(TopologyGraphNode node)
        {
            foreach (var child in node.ChildBranches.Select(x => x.DownStream))
            {
                TopologyGraphNode childNode = child as TopologyGraphNode;

                if (IsNodeConnectedToItsParent(childNode, node.Item))
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsNodeConnectedToItsParent(TopologyGraphNode node, long parentGid)
        {
            foreach (var child in node.ChildBranches.Select(x => x.DownStream))
            {
                TopologyGraphNode childNode = child as TopologyGraphNode;

                if (childNode.Item == parentGid)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
