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

        private Thread discreteValueAligner;
        private CancellationTokenSource cancellationTokenSource;
        private AutoResetEvent commitedEvent;

        public TopologyAnalysis(IStorage<DiscreteRemotePoint> discreteRemotePointStorage, BreakerMessageMapping breakerMessageMapping) : base()
        {
            graphLocker = new ReaderWriterLockSlim();

            topologyModifier = new TopologyModifier(this, discreteRemotePointStorage);

            this.breakerMessageMapping = breakerMessageMapping;
            this.discreteRemotePointStorage = discreteRemotePointStorage;

            cancellationTokenSource = new CancellationTokenSource();
            discreteValueAligner = new Thread(() => AlignBreakerStates(cancellationTokenSource.Token));
            discreteValueAligner.Start();
        }

        public void ChangeBreakerValue(long breakerGid, int rawBreakerValue)
        {
            foreach (var graph in graphs)
            {
                List<TopologyBreakerGraphBranch> branches = graph.Value.GetBreakerBranches(breakerGid);

                if (branches == null)
                {
                    continue;
                }

                foreach (var branch in branches)
                {
                    branch.BreakerState = breakerMessageMapping.MapRawDataToBreakerState(rawBreakerValue);
                }              
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

        public IEnumerable<ISubscription> GetSubscriptions()
        {
            return new List<ISubscription>() { new Subscription(Topic.DiscreteRemotePointChange, new BreakerStateChangedTopologyAnalysisDynamicHandler(topologyModifier, discreteRemotePointStorage)) };
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

        public override AutoResetEvent AlignEvent
        {
            set { commitedEvent = value; }
        }

        protected override IEnumerable<long> GetRootsGlobalId(TopologyGraph graph)
        {
            return graph.GetRoots().Select(x => x.Item);
        }

        private void AlignBreakerStates(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                discreteRemotePointStorage.Commited.WaitOne();
                commitedEvent.WaitOne();

                if (cancellationToken.IsCancellationRequested)
                {
                    continue;
                }

                foreach (var discreteRemotePoint in discreteRemotePointStorage.GetAllEntities())
                {
                    ChangeBreakerValue(discreteRemotePoint.BreakerGid, discreteRemotePoint.Value);
                }
            }
        }
    }
}
