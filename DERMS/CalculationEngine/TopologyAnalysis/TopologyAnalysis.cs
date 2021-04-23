using CalculationEngine.Graphs.GraphProcessors;
using CalculationEngine.Model.Topology.Graph.Topology;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Common.ComponentStorage;
using CalculationEngine.Model.Topology.Transaction;
using Common.PubSub;
using Common.PubSub.Subscriptions;
using CalculationEngine.PubSub.DynamicHandlers;
using Common.Helpers.Breakers;
using Common.ServiceInterfaces.CalculationEngine;
using System;

namespace CalculationEngine.TopologyAnalysis
{
    public class TopologyAnalysis : BaseGraphProcessor<TopologyGraph>, ITopologyAnalysis, ITopologyAnalysisBreakerManipulator, ISubscriber
    {
        private ReaderWriterLockSlim graphLocker;
        private BreakerMessageMapping breakerMessageMapping;
        private IStorage<DiscreteRemotePoint> discreteRemotePointStorage;
        private ITopologyModifier topologyModifier;

        private IBreakerLoopCommandingValidator commandingLoopValidator;
        private InterConnectedBreakerCommanding.IInterConnectedBreakerCommanding interConnectedBreakerCommanding;

        private Thread discreteValueAligner;
        private CancellationTokenSource cancellationTokenSource;
        private AutoResetEvent commitedEvent;
        private AutoResetEvent readyEvent;

        public TopologyAnalysis(IStorage<DiscreteRemotePoint> discreteRemotePointStorage, BreakerMessageMapping breakerMessageMapping) : base()
        {
            readyEvent = new AutoResetEvent(false);
            graphLocker = new ReaderWriterLockSlim();

            topologyModifier = new TopologyModifier(this, discreteRemotePointStorage);

            interConnectedBreakerCommanding = new InterConnectedBreakerCommanding.InterConnectedBreakerCommanding(this, discreteRemotePointStorage, breakerMessageMapping);

            this.breakerMessageMapping = breakerMessageMapping;
            this.discreteRemotePointStorage = discreteRemotePointStorage;

            cancellationTokenSource = new CancellationTokenSource();
            discreteValueAligner = new Thread(() => AlignBreakerStates(cancellationTokenSource.Token));
            discreteValueAligner.Start();
        }

        public void ChangeBreakerValue(long breakerGid, int rawBreakerValue, bool initialization = false)
        {
            bool commandsCreatesLoop = commandingLoopValidator.ValidateCommand(breakerGid, breakerMessageMapping.MapRawDataToBreakerState(rawBreakerValue));

            if (!initialization && commandsCreatesLoop)
            {
                // TODO COMMAND TO OPEN COMMANDED BREAKER OR INTERCONNECTED BREAKER
                return;
            }

            foreach (var graph in graphs)
            {
                List<TopologyBreakerGraphBranch> branches = graph.Value.GetBreakerBranches(breakerGid);

                if (branches == null)
                {
                    continue;
                }

                if (!initialization)
                {
                    interConnectedBreakerCommanding.ProcessBreakerCommanding(breakerGid, rawBreakerValue);
                }

                // skip inter connected breaker branches
                if (initialization || branches.Count == 1)
                {
                    foreach (var branch in branches)
                    {
                        branch.BreakerState = breakerMessageMapping.MapRawDataToBreakerState(rawBreakerValue);
                    }
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

        public List<TopologyGraphNode> GetRoots()
        {
            List<TopologyGraphNode> roots = new List<TopologyGraphNode>();
            graphLocker.EnterReadLock();

            roots.AddRange(graphs.Values.First().GetRoots());

            graphLocker.ExitReadLock();

            return roots;

        }

        public IBreakerLoopCommandingValidator BreakerLoopCommandingValidator { set { commandingLoopValidator = value; } }

        public AutoResetEvent ReadyEvent { get { return readyEvent; } }

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

                commandingLoopValidator.UpdateBreakers();
                interConnectedBreakerCommanding.UpdateBreakers();

                foreach (var discreteRemotePoint in discreteRemotePointStorage.GetAllEntities())
                {
                    ChangeBreakerValue(discreteRemotePoint.BreakerGid, discreteRemotePoint.Value, true);
                }

                readyEvent.Set();
            }
        }
    }
}
