using CalculationEngine.Graphs.GraphProcessors;
using CalculationEngine.Model.Topology.Graph;
using CalculationEngine.Model.Topology.Graph.Topology;
using System.Collections.Generic;
using System.Linq;

namespace CalculationEngine.TopologyAnalysis
{
    public class TopologyAnalysis : BaseGraphProcessor<IMultipleRootGraph<TopologyGraphNode>>
    {
        protected override IEnumerable<long> GetRootsGlobalId(IMultipleRootGraph<TopologyGraphNode> graph)
        {
            return graph.GetRoots().Select(x => x.Item);
        }
    }
}
