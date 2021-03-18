using CalculationEngine.Model.Topology.Graph;
using System.Collections.Generic;
using Common.AbstractModel;
using System.Linq;

namespace CalculationEngine.Graphs.GraphReductionRules
{
    public abstract class ReduceACLineSegmentBranchesGraphRule<GraphNodeType> : GraphReductionRule<GraphNodeType>
        where GraphNodeType : DMSTypeGraphNode
    {
        protected BaseGraphBranchManipulator<GraphNodeType> graphBranchManipulator;

        protected ReduceACLineSegmentBranchesGraphRule(BaseGraphBranchManipulator<GraphNodeType> graphBranchManipulator) : base(new List<DMSType>() { DMSType.CONNECTIVITYNODE })
        {
            this.graphBranchManipulator = graphBranchManipulator;
        }

        protected override void ApplyRule(GraphNodeType node, IGraph<GraphNodeType> graph)
        {
            foreach (var childBranch in node.ChildBranches.ToList())
            {
                GraphNodeType child = childBranch.DownStream as GraphNodeType;

                if (!IsNeededNeighbour(child))
                {
                    continue;
                }

                foreach (var grandChildBranch in child.ChildBranches.ToList())
                {
                    GraphNodeType grandChild = grandChildBranch.DownStream as GraphNodeType;
                    AddNewBranch(node, grandChild, child);

                    graphBranchManipulator.DeleteBranch(grandChildBranch);
                }

                graphBranchManipulator.DeleteBranch(childBranch);

                graph.RemoveNode(child.Item);
            }
        }

        protected virtual bool IsNeededNeighbour(GraphNodeType node)
        {
            return node.DMSType == DMSType.ACLINESEG;
        }

        protected virtual GraphBranch<GraphNode> AddNewBranch(GraphNodeType parent, GraphNodeType child, GraphNodeType branch)
        {
            return graphBranchManipulator.AddBranch(parent, child);
        }
    }
}
