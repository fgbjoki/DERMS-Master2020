using CalculationEngine.Graphs;
using System.Collections.Generic;

namespace CalculationEngine.Model.Topology.Graph.Topology
{
    public class TopologyGraph : BaseMultipleRootGraph<TopologyGraphNode>
    {
        private Dictionary<long, TopologyBreakerGraphBranch> breakerBranches;

        public TopologyGraph() : base()
        {
            breakerBranches = new Dictionary<long, TopologyBreakerGraphBranch>();
        }

        public void LoadBreakerBranches(IEnumerable<TopologyBreakerGraphBranch> breakerBranches)
        {
            foreach (var breakerBranch in breakerBranches)
            {
                this.breakerBranches.Add(breakerBranch.BreakerGlobalId, breakerBranch);
            }
        }

        public TopologyBreakerGraphBranch GetBreakerBranch(long breakerGid)
        {
            TopologyBreakerGraphBranch branch;

            breakerBranches.TryGetValue(breakerGid, out branch);

            return branch;
        }
    }
}
