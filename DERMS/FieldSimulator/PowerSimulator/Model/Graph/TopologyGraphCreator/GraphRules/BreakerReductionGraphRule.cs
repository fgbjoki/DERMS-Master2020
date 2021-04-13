using System.Collections.Generic;
using System.Linq;
using Common.AbstractModel;
using FieldSimulator.PowerSimulator.Model.Graph.Graphs.Branches;
using FieldSimulator.PowerSimulator.Model.Graph.GraphManipulators;
using FieldSimulator.PowerSimulator.Model.Graph.Graphs.Nodes;

namespace FieldSimulator.PowerSimulator.Model.Graph.TopologyGraphCreator.GraphRules
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
            breakerBranch.BreakerGlobalId = branch.GlobalId;

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

                if (IsNodeConnectedToItsParent(childNode, node.GlobalId))
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

                if (childNode.GlobalId == parentGid)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
