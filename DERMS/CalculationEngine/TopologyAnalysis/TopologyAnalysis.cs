using CalculationEngine.Graphs.GraphProcessors;
using CalculationEngine.Model.Topology.Graph.Topology;
using System.Collections.Generic;
using System.Linq;
using Common.AbstractModel;
using System.Threading;
using CalculationEngine.Model.Topology;

namespace CalculationEngine.TopologyAnalysis
{
    public class TopologyAnalysis : BaseGraphProcessor<TopologyGraph>, ITopologyAnalysis, ITopologyAnalysisBreakerManipulator
    {
        private ReaderWriterLockSlim graphLocker;
        private BreakerMessageMapping breakerMessageMapping;

        public TopologyAnalysis() : base()
        {
            graphLocker = new ReaderWriterLockSlim();
        }

        public void ChangeBreakerValue(long breakerGid, int rawBreakerValue)
        {
            foreach (var graph in graphs)
            {
                TopologyBreakerGraphBranch branch = graph.Value.GetBreakerBranch(breakerGid);

                if (branch == null)
                {
                    continue;
                }

                branch.BreakerState = breakerMessageMapping.MapRawDataToBreakerState(rawBreakerValue);
            }
        }

        public ITopologyReader CreateReader(List<DMSType> typesToConsider)
        {
            TopologyReader topologyReader = new TopologyReader(this, typesToConsider);

            return topologyReader;
        }

        public ITopologyModifier CreateWriter()
        {
            return new TopologyModifier(this);
        }

        public ReaderWriterLockSlim GetLock()
        {
            return graphLocker;
        }

        public TopologyGraphNode GetRoot(long rootGlobalId)
        {
            TopologyGraph graph;

            if (!graphs.TryGetValue(rootGlobalId, out graph))
            {
                return null;
            }

            return graph.GetNode(rootGlobalId);
        }

        protected override IEnumerable<long> GetRootsGlobalId(TopologyGraph graph)
        {
            return graph.GetRoots().Select(x => x.Item);
        }
    }
}
