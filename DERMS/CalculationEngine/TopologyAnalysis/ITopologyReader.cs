using System.Collections.Generic;

namespace CalculationEngine.TopologyAnalysis
{
    public interface ITopologyReader
    {
        IEnumerable<long> Read(long sourceGid);
    }
}
