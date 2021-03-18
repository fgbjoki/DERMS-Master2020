using CalculationEngine.Model.Topology.Graph.Topology;
using System.Collections.Generic;
using Common.AbstractModel;
using CalculationEngine.Model.Topology.Graph;
using System.Linq;

namespace CalculationEngine.Graphs.GraphReductionRules.Topology
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

                node.Shunts.Add(new Shunt(child.Item));

                graphBranchManipulator.DeleteBranch(childBranch);
                graph.RemoveNode(child.Item);
            }
        }

        private bool IsNeededNeighbour(TopologyGraphNode neighbour)
        {
            return shuntDMSTypes.Contains(neighbour.DMSType);
        }
    }
}
