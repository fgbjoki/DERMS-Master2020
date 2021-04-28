using System.Collections.Generic;
using System.Linq;
using CalculationEngine.TopologyAnalysis;
using System.Threading;
using CalculationEngine.Model.Topology.Transaction;
using Common.ComponentStorage;
using CalculationEngine.Helpers.Topology;
using Common.Helpers.Breakers;
using Common.ServiceInterfaces.CalculationEngine;

namespace CalculationEngine.Commanding.BreakerCommanding
{
    public class BreakerCommandingUnit : IBreakerCommanding
    {
        private ITopologyAnalysis topologyAnalysis;
        private ReaderWriterLockSlim locker;
        private BreakerLoopFinder breakerLoopFinder;
        private IStorage<DiscreteRemotePoint> discreteRemotePoint;

        private List<long> breakerPathBetweenSources;

        private BreakerMessageMapping breakerMessageMapping;

        public BreakerCommandingUnit(ITopologyAnalysis topologyAnalysis, IStorage<DiscreteRemotePoint> discreteRemotePoint, BreakerMessageMapping breakerMessageMapping)
        {
            this.topologyAnalysis = topologyAnalysis;
            this.discreteRemotePoint = discreteRemotePoint;
            this.breakerMessageMapping = breakerMessageMapping;

            locker = new ReaderWriterLockSlim();

            breakerLoopFinder = new BreakerLoopFinder();
        }

        public ITopologyAnalysis TopologyAnalysis { set { topologyAnalysis = value; } }

        public void UpdateBreakers()
        {
            locker.EnterWriteLock();

            breakerPathBetweenSources = breakerLoopFinder.GetBreakerLoops(topologyAnalysis.GetRoots().First());

            locker.ExitWriteLock();
        }

        public bool ValidateCommand(long commandingBreakerGid, BreakerState breakerState)
        {
            bool allBreakersConduct = true;

            locker.EnterReadLock();
            try
            {
                foreach (var breakerGid in breakerPathBetweenSources)
                {
                    BreakerState currentBreakerState;

                    if (breakerGid == commandingBreakerGid)
                    {
                        currentBreakerState = breakerState;
                    }
                    else
                    {
                        currentBreakerState = breakerMessageMapping.MapRawDataToBreakerState(discreteRemotePoint.GetEntity(breakerGid).Value);
                    }

                    allBreakersConduct &= currentBreakerState == BreakerState.CLOSED ? true : false;
                }
            }
            finally
            {
                locker.ExitReadLock();
            }

            return !allBreakersConduct;
        }

        public bool SendCommand(long breakerGid, int breakerValue)
        {
            if (!ValidateCommand(breakerGid, breakerMessageMapping.MapRawDataToBreakerState(breakerValue)))
            {
                return false;
            }

            // TODO send command to NDS

            return true;
        }
    }
}
