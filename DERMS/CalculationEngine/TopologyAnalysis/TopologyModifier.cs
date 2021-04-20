using CalculationEngine.Model.Topology.Transaction;
using Common.ComponentStorage;
using Common.Logger;
using System.Threading;

namespace CalculationEngine.TopologyAnalysis
{
    public class TopologyModifier : ITopologyModifier
    {
        private ITopologyAnalysisBreakerManipulator breakerManipulator;

        private IStorage<DiscreteRemotePoint> discreteStorage;

        public TopologyModifier(ITopologyAnalysisBreakerManipulator breakerManipulator, IStorage<DiscreteRemotePoint> discreteStorage)
        {
            this.breakerManipulator = breakerManipulator;
            this.discreteStorage = discreteStorage;
        }

        public void Write(long breakerGid, int rawValue)
        {
            ReaderWriterLockSlim locker = breakerManipulator.GetLock();

            locker.EnterWriteLock();

            breakerManipulator.ChangeBreakerValue(breakerGid, rawValue);

            locker.ExitWriteLock();
        }
    }
}
