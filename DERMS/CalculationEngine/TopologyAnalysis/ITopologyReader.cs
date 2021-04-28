using System.Collections.Generic;

namespace CalculationEngine.TopologyAnalysis
{
    public interface ITopologyReader
    {
        ICollection<long> Read(long sourceGid);
        long FindSource(long nodeGid);
    }
}
