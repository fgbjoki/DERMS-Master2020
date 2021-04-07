using System.Threading;

namespace CalculationEngine.TopologyAnalysis
{
    public interface ITopologyAnalysisBreakerManipulator
    {
        ReaderWriterLockSlim GetLock();
        void ChangeBreakerValue(long breakerGid, int rawBreakerValue);
    }
}
