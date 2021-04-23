using CalculationEngine.Graphs;
using System.Collections.Generic;

namespace CalculationEngine.Model.Topology.Graph.Topology
{
    public class TopologyGraph : BaseMultipleRootGraph<TopologyGraphNode>
    {
        private Dictionary<long, List<TopologyBreakerGraphBranch>> breakerBranches;
        private Dictionary<long, long> shuntToNodeMap;

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

        public void LoadShuntReductionMap(Dictionary<long, long> shuntToNodeMap)
        {
            this.shuntToNodeMap = shuntToNodeMap;
        }

        public List<TopologyBreakerGraphBranch> GetBreakerBranches(long breakerGid)
        {
            List<TopologyBreakerGraphBranch> branches;

            breakerBranches.TryGetValue(breakerGid, out branches);

            return branches;
        }

        public override TopologyGraphNode GetNode(long globalId)
        {
            long correctedGid = 0;

            if (shuntToNodeMap != null)
            {
                shuntToNodeMap.TryGetValue(globalId, out correctedGid);
            }

            if (correctedGid == 0)
            {
                correctedGid = globalId;
            }

            return base.GetNode(correctedGid);
        }
    }
}
