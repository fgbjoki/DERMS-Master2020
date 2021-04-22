using CalculationEngine.Model.Topology;
using CalculationEngine.Model.Topology.Graph.Topology;
using CalculationEngine.Model.Topology.Transaction;
using Common.ComponentStorage;
using Common.Helpers.Breakers;
using System.Collections.Generic;

namespace CalculationEngine.TopologyAnalysis.InterConnectedBreakerCommanding
{
    class BreakersToInterConnectedBranchMapping
    {
        private HashSet<long> breakersInvolved;
        private TopologyBreakerGraphBranch breakerBranch;

        public BreakersToInterConnectedBranchMapping(IEnumerable<long> breakersInvolved, TopologyBreakerGraphBranch breakerBranch)
        {
            this.breakersInvolved = new HashSet<long>(breakersInvolved);
            this.breakerBranch = breakerBranch;
        }

        public BreakerState GetBranchCommand(long commandedBreaker, IStorage<DiscreteRemotePoint> discreteRemotePoint, BreakerMessageMapping breakerMessageMapping)
        {
            bool shouldCloseBranch = IsBreakerInvolved(commandedBreaker) && ShouldPropagateChanges(discreteRemotePoint, breakerMessageMapping);

            return shouldCloseBranch ? BreakerState.CLOSED : BreakerState.OPEN;
        }

        public TopologyBreakerGraphBranch BreakerBranch { get { return breakerBranch; } }

        public bool IsBreakerInvolved(long breakerGid)
        {
            return breakersInvolved.Contains(breakerGid);
        }

        private bool ShouldPropagateChanges(IStorage<DiscreteRemotePoint> discreteRemotePoint, BreakerMessageMapping breakerMessageMapping)
        {
            foreach (var breakerInvolved in breakersInvolved)
            {
                int rawBreakerValue = discreteRemotePoint.GetEntity(breakerInvolved).Value;
                if (breakerMessageMapping.MapRawDataToBreakerState(rawBreakerValue) == BreakerState.OPEN)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
