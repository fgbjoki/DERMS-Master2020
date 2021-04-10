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

        public void Write(long discreteRemotePointGid, int rawValue)
        {
            DiscreteRemotePoint discretePoint = discreteStorage.GetEntity(discreteRemotePointGid);

            if (discretePoint == null)
            {
                Logger.Instance.Log($"[{GetType().Name}] Couldn't find discrete remote point with gid: {discretePoint.BreakerGid:X16}. Topology state might be in fault.");
                return;
            }

            ReaderWriterLockSlim locker = breakerManipulator.GetLock();

            locker.EnterWriteLock();

            breakerManipulator.ChangeBreakerValue(discretePoint.BreakerGid, rawValue);

            locker.ExitWriteLock();
        }
    }
}
