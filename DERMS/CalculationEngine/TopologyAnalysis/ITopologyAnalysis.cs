using CalculationEngine.Model.Topology.Graph.Topology;
using Common.AbstractModel;
using System.Collections.Generic;
using System.Threading;

namespace CalculationEngine.TopologyAnalysis
{
    public interface ITopologyAnalysis
    {
        ITopologyReader CreateReader(List<DMSType> typesToConsider);
        ITopologyWriter CreateWriter();
        TopologyGraphNode GetRoot(long rootGlobalId);
        ReaderWriterLockSlim GetLock();
    }
}
