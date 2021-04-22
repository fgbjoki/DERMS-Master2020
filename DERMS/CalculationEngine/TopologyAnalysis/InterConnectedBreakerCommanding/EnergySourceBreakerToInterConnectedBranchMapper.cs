using CalculationEngine.Model.Topology.Graph.Topology;
using System.Collections.Generic;
using System.Linq;
using CalculationEngine.Model.Topology.Graph;

namespace CalculationEngine.TopologyAnalysis.InterConnectedBreakerCommanding
{
    class EnergySourceBreakerToInterConnectedBranchMapper
    {
        public List<BreakersToInterConnectedBranchMapping> CreateMapping(List<long> loopBreakerGids, IEnumerable<TopologyGraphNode> roots)
        {
            List<BreakersToInterConnectedBranchMapping> mappings = new List<BreakersToInterConnectedBranchMapping>();

            foreach (var root in roots)
            {
                mappings.Add(ProcessRoot(root, loopBreakerGids));
            }

            return mappings;
        }

        private BreakersToInterConnectedBranchMapping ProcessRoot(TopologyGraphNode root, List<long> loopBreakerGids)
        {
            List<long> breakerGidsInvolved = new List<long>(2);

            TopologyGraphNode currentNode = root;

            while (currentNode.ParentBranches.Count <= 1)
            {
                var branch = currentNode.ChildBranches.First();

                ProcessBranch(branch as TopologyBreakerGraphBranch, breakerGidsInvolved, loopBreakerGids);

                currentNode = branch.DownStream as TopologyGraphNode;
            }

            TopologyBreakerGraphBranch dependentBranch = null;

            foreach (var branch in currentNode.ChildBranches)
            {
                TopologyGraphNode neighbour = branch.UpStream as TopologyGraphNode;

                if (neighbour.ParentBranches.Count == 1)
                {
                    continue;
                }

                if (!ProcessBranch(branch as TopologyBreakerGraphBranch, breakerGidsInvolved, loopBreakerGids))
                {
                    continue;
                }

                MarkDepdenentBreakerBranch(ref dependentBranch, branch, breakerGidsInvolved);

                break;
            }

            return new BreakersToInterConnectedBranchMapping(breakerGidsInvolved, dependentBranch);
        }

        private void MarkDepdenentBreakerBranch(ref TopologyBreakerGraphBranch dependentBranch, GraphBranch<GraphNode> branch, List<long> breakerGidsInvolved)
        {
            TopologyBreakerGraphBranch breakerBranch = branch as TopologyBreakerGraphBranch;

            if (breakerBranch == null)
            {
                return;
            }

            if (breakerGidsInvolved.Contains(breakerBranch.BreakerGlobalId))
            {
                dependentBranch = breakerBranch;
            }
        }

        private bool ProcessBranch(TopologyBreakerGraphBranch branch, List<long> breakerGidsInvolved, List<long> loopBreakerGids)
        {
            if (branch == null)
            {
                return false;
            }

            if (loopBreakerGids.Contains(branch.BreakerGlobalId))
            {
                breakerGidsInvolved.Add(branch.BreakerGlobalId);
            }

            return true;
        }
    }
}
