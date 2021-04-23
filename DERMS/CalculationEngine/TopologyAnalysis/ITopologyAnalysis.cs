using CalculationEngine.Model.Topology.Graph.Topology;
using System.Collections.Generic;
using System.Threading;

namespace CalculationEngine.TopologyAnalysis
{
    public interface ITopologyAnalysis
    {
        ITopologyReader CreateReader();
        ITopologyModifier GetModifier();
        TopologyGraphNode GetRoot(long rootGlobalId);
        List<TopologyGraphNode> GetRoots();
        TopologyGraphNode GetNode(long nodeGlobalId);
        ReaderWriterLockSlim GetLock();

        AutoResetEvent ReadyEvent { get; }
    }
}
