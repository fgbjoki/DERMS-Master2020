using CalculationEngine.Graphs.GraphProcessors;
using CalculationEngine.Model.Topology.Graph.Topology;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CalculationEngine.Model.Topology;
using Common.ComponentStorage;
using CalculationEngine.Model.Topology.Transaction;
using Common.PubSub;
using Common.PubSub.Subscriptions;
using CalculationEngine.PubSub.DynamicHandlers;

namespace CalculationEngine.TopologyAnalysis
{
    public class TopologyAnalysis : BaseGraphProcessor<TopologyGraph>, ITopologyAnalysis, ITopologyAnalysisBreakerManipulator, ISubscriber
    {
        private ReaderWriterLockSlim graphLocker;
        private BreakerMessageMapping breakerMessageMapping;
        private IStorage<DiscreteRemotePoint> discreteRemotePointStorage;
        private ITopologyModifier topologyModifier;

        public TopologyAnalysis(IStorage<DiscreteRemotePoint> discreteRemotePointStorage) : base()
        {
            graphLocker = new ReaderWriterLockSlim();

            topologyModifier = new TopologyModifier(this, discreteRemotePointStorage);

            this.discreteRemotePointStorage = discreteRemotePointStorage;
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

        public ITopologyReader CreateReader()
        {
            TopologyReader topologyReader = new TopologyReader(this);

            return topologyReader;
        }

        public ITopologyModifier GetModifier()
        {
            return topologyModifier;
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

        public IEnumerable<ISubscription> GetSubscriptions()
        {
            return new List<ISubscription>() { new Subscription(Topic.DiscreteRemotePointChange, new BreakerStateChangedTopologyAnalysisDynamicHandler(topologyModifier)) };
        }

        public TopologyGraphNode GetNode(long nodeGlobalId)
        {
            foreach (var graph in graphs)
            {
                TopologyGraphNode node = graph.Value.GetNode(nodeGlobalId);
                if (node != null)
                {
                    return node;
                }
            }

            return null;
        }
    }
}
