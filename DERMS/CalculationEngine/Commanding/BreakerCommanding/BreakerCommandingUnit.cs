using System.Collections.Generic;
using System.Linq;
using CalculationEngine.TopologyAnalysis;
using System.Threading;
using CalculationEngine.Model.Topology.Transaction;
using Common.ComponentStorage;
using CalculationEngine.Helpers.Topology;
using Common.Helpers.Breakers;
using Common.ServiceInterfaces.CalculationEngine;
using Common.Communication;
using Common.ServiceInterfaces.NetworkDynamicsService.Commands;
using System;
using Common.Logger;

namespace CalculationEngine.Commanding.BreakerCommanding
{
    public class BreakerCommandingUnit : IBreakerCommanding
    {
        private ITopologyAnalysis topologyAnalysis;
        private ReaderWriterLockSlim locker;
        private BreakerLoopFinder breakerLoopFinder;
        private IStorage<DiscreteRemotePoint> discreteRemotePointStorage;

        private List<long> breakerPathBetweenSources;

        private BreakerMessageMapping breakerMessageMapping;

        private WCFClient<INDSCommanding> ndsCommandingClient;

        public BreakerCommandingUnit(ITopologyAnalysis topologyAnalysis, IStorage<DiscreteRemotePoint> discreteRemotePointStorage, BreakerMessageMapping breakerMessageMapping)
        {
            this.topologyAnalysis = topologyAnalysis;
            this.discreteRemotePointStorage = discreteRemotePointStorage;
            this.breakerMessageMapping = breakerMessageMapping;

            locker = new ReaderWriterLockSlim();

            breakerLoopFinder = new BreakerLoopFinder();

            ndsCommandingClient = new WCFClient<INDSCommanding>("ndsCommanding");
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
                        currentBreakerState = breakerMessageMapping.MapRawDataToBreakerState(discreteRemotePointStorage.GetEntity(breakerGid).Value);
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

            DiscreteRemotePoint remotePoint = discreteRemotePointStorage.GetEntity(breakerGid);
            if (remotePoint == null)
            {
                Logger.Instance.Log($"[{GetType().Name}] Couldn't find remote point connected to breaker with gid: 0x{breakerGid:X16}.");
                return false;
            }

            try
            {
                return ndsCommandingClient.Proxy.SendCommand(new ChangeDiscreteRemotePointValue(remotePoint.GlobalId, breakerValue));
            }
            catch (Exception e)
            {
                Logger.Instance.Log($"[{GetType().Name}] Couldn't send command to NDS. More info:\n{e.Message}\nStack trace:\n{e.StackTrace}");
                return false;
            }
        }
    }
}
