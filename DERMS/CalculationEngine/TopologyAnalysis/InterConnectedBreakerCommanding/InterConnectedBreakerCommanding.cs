using CalculationEngine.Helpers.Topology;
using CalculationEngine.Model.Topology;
using CalculationEngine.Model.Topology.Graph.Topology;
using CalculationEngine.Model.Topology.Transaction;
using Common.ComponentStorage;
using Common.Helpers.Breakers;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace CalculationEngine.TopologyAnalysis.InterConnectedBreakerCommanding
{
    class InterConnectedBreakerCommanding : IInterConnectedBreakerCommanding
    {
        private ITopologyAnalysis topologyAnalysis;
        private ReaderWriterLockSlim locker;
        private BreakerLoopFinder breakerLoopFinder;
        private IStorage<DiscreteRemotePoint> discreteRemotePointStorage;

        private BreakerMessageMapping breakerMessageMapping;

        private List<BreakersToInterConnectedBranchMapping> mappings;
        private EnergySourceBreakerToInterConnectedBranchMapper mappingCreator;

        public InterConnectedBreakerCommanding(ITopologyAnalysis topologyAnalysis, IStorage<DiscreteRemotePoint> discreteRemotePointStorage, BreakerMessageMapping breakerMessageMapping)
        {
            this.topologyAnalysis = topologyAnalysis;
            this.discreteRemotePointStorage = discreteRemotePointStorage;
            this.breakerMessageMapping = breakerMessageMapping;

            breakerLoopFinder = new BreakerLoopFinder();
            mappingCreator = new EnergySourceBreakerToInterConnectedBranchMapper();
            locker = new ReaderWriterLockSlim();
        }

        public void UpdateBreakers()
        {
            locker.EnterWriteLock();

            List<long> loopBreakerGids = breakerLoopFinder.GetBreakerLoops(topologyAnalysis.GetRoots().First());
            mappings = mappingCreator.CreateMapping(loopBreakerGids, topologyAnalysis.GetRoots());

            locker.ExitWriteLock();
        }

        public void ProcessBreakerCommanding(long breakerGidCommanding, int rawBreakerValue)
        {
            locker.EnterReadLock();

            foreach (var mapping in mappings)
            {
                if (!mapping.IsBreakerInvolved(breakerGidCommanding))
                {
                    continue;
                }

                BreakerState newBreakerState = mapping.GetBranchCommand(breakerGidCommanding, discreteRemotePointStorage, breakerMessageMapping);

                TopologyBreakerGraphBranch breakerBranch = mapping.BreakerBranch;
                breakerBranch.BreakerState = newBreakerState;
            }

            locker.ExitReadLock();
        }
    }
}
