using CalculationEngine.Model.Topology.Graph;
using CalculationEngine.Model.Topology.Graph.Topology;
using System.Collections.Generic;
using System.Linq;

namespace CalculationEngine.Helpers.Topology
{
    class BreakerLoopFinder
    {
        public List<long> GetBreakerLoops(TopologyGraphNode root)
        {
            List<long> breakerGids = new List<long>();

            TopologyGraphNode interConnectedConnectivityNode = PopulateBreakerGidsUntilInterConnectedConnectivityNode(root, breakerGids);
            interConnectedConnectivityNode = ProcessSecondInterConnectedConnectivityNode(interConnectedConnectivityNode, breakerGids);
            ProcessBreakersInSourceDirection(interConnectedConnectivityNode, breakerGids);

            return breakerGids;
        }

        private TopologyGraphNode ProcessSecondInterConnectedConnectivityNode(TopologyGraphNode interConnectedConnectivityNode, List<long> breakerGids)
        {
            foreach (var branch in interConnectedConnectivityNode.ParentBranches)
            {
                TopologyGraphNode neighbourNode = branch.UpStream as TopologyGraphNode;
                if (!neighbourNode.ParentBranches.Select(x => x.UpStream).Contains(interConnectedConnectivityNode))
                {
                    continue;
                }

                ProcessBranch(branch as TopologyGraphBranch, breakerGids);

                return neighbourNode;
            }

            return null;
        }

        private TopologyGraphNode PopulateBreakerGidsUntilInterConnectedConnectivityNode(TopologyGraphNode node, List<long> breakerGids)
        {
            GraphNode currentNode = node;

            while (currentNode.ParentBranches.Count <= 1)
            {
                TopologyGraphBranch branch = currentNode.ChildBranches.First() as TopologyGraphBranch;
                ProcessBranch(branch, breakerGids);
                currentNode = branch.DownStream;
            }

            return currentNode as TopologyGraphNode;
        }

        private void ProcessBreakersInSourceDirection(TopologyGraphNode node, List<long> breakerGids)
        {
            GraphNode currentNode = node;

            while (currentNode.ParentBranches.Count != 0)
            {
                TopologyGraphBranch branch = currentNode.ParentBranches.First() as TopologyGraphBranch;
                ProcessBranch(branch, breakerGids);
                currentNode = branch.UpStream;
            }
        }

        private void ProcessBranch(TopologyGraphBranch branch, List<long> breakerGids)
        {
            TopologyBreakerGraphBranch breakerBranch = branch as TopologyBreakerGraphBranch;

            if (breakerBranch == null)
            {
                return;
            }

            breakerGids.Add(breakerBranch.BreakerGlobalId);
        }
    }
}
