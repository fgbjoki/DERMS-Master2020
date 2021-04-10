using CalculationEngine.Model.Topology.Graph.Topology;
using System.Threading;

namespace CalculationEngine.TopologyAnalysis
{
    public interface ITopologyAnalysis
    {
        ITopologyReader CreateReader();
        ITopologyModifier GetModifier();
        TopologyGraphNode GetRoot(long rootGlobalId);
        TopologyGraphNode GetNode(long nodeGlobalId);
        ReaderWriterLockSlim GetLock();
    }
}
