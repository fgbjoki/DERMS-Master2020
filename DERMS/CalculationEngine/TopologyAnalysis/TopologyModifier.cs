using System.Threading;

namespace CalculationEngine.TopologyAnalysis
{
    public class TopologyModifier : ITopologyModifier
    {
        private ITopologyAnalysisBreakerManipulator breakerManipulator;

        public TopologyModifier(ITopologyAnalysisBreakerManipulator breakerManipulator)
        {
            this.breakerManipulator = breakerManipulator;
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
