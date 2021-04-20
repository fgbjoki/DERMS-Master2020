using CalculationEngine.Graphs;
using System.Collections.Generic;

namespace CalculationEngine.Model.Topology.Graph.Topology
{
    public class TopologyGraph : BaseMultipleRootGraph<TopologyGraphNode>
    {
        private Dictionary<long, List<TopologyBreakerGraphBranch>> breakerBranches;

        public TopologyGraph() : base()
        {
            breakerBranches = new Dictionary<long, List<TopologyBreakerGraphBranch>>();
        }

        public void LoadBreakerBranches(IEnumerable<TopologyBreakerGraphBranch> breakerBranches)
        {
            foreach (var breakerBranch in breakerBranches)
            {
                List<TopologyBreakerGraphBranch> branches;
                if (!this.breakerBranches.TryGetValue(breakerBranch.BreakerGlobalId, out branches))
                {
                    branches = new List<TopologyBreakerGraphBranch>();
                    this.breakerBranches.Add(breakerBranch.BreakerGlobalId, branches);
                }

                branches.Add(breakerBranch);
            }
        }

        public List<TopologyBreakerGraphBranch> GetBreakerBranches(long breakerGid)
        {
            List<TopologyBreakerGraphBranch> branches;

            breakerBranches.TryGetValue(breakerGid, out branches);

            return branches;
        }
    }
}
