using System.Collections.Generic;
using Common.AbstractModel;
using System.Linq;
using FieldSimulator.PowerSimulator.Model.Graph.GraphManipulators;
using FieldSimulator.PowerSimulator.Model.Graph.Graphs.Nodes;

namespace FieldSimulator.PowerSimulator.Model.Graph.TopologyGraphCreator.GraphRules
{
    public class ConnectivityNodeShuntReductionGraphRule : GraphReductionRule<TopologyGraphNode>
    {
        private List<DMSType> shuntDMSTypes;

        private TopologyGraphBranchManipulator graphBranchManipulator;

        public ConnectivityNodeShuntReductionGraphRule(TopologyGraphBranchManipulator graphBranchManipulator) : base(new List<DMSType>() { DMSType.CONNECTIVITYNODE })
        {
            this.graphBranchManipulator = graphBranchManipulator;

            shuntDMSTypes = new List<DMSType>()
            {
                DMSType.ENERGYCONSUMER,
                DMSType.ENERGYSTORAGE
            };
        }

        protected override void ApplyRule(TopologyGraphNode node, IGraph<TopologyGraphNode> graph)
        {
            foreach (var childBranch in node.ChildBranches.ToList())
            {
                TopologyGraphNode child = childBranch.DownStream as TopologyGraphNode;

                if (!IsNeededNeighbour(child))
                {
                    continue;
                }

                node.Shunts.Add(new Shunt(child.GlobalId));

                graphBranchManipulator.DeleteBranch(childBranch);
                graph.RemoveNode(child.GlobalId);
            }
        }

        private bool IsNeededNeighbour(TopologyGraphNode neighbour)
        {
            return shuntDMSTypes.Contains(neighbour.DMSType);
        }
    }
}
