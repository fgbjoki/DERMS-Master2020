using CalculationEngine.Model.Topology.Graph.Topology;
using Common.Logger;
using System.Collections.Generic;
using System.Threading;
using CalculationEngine.TopologyAnalysis.GraphTraversing;

namespace CalculationEngine.TopologyAnalysis
{
    public class TopologyReader : ITopologyReader
    {
        private ITopologyAnalysis topologyAnalysis;
        private TopologyGraphTraverser topologyGraphTraverser;
        private TopologyGraphSourceFinder sourceFinder;

        public TopologyReader(ITopologyAnalysis topologyAnalysis)
        {
            this.topologyAnalysis = topologyAnalysis;

            topologyGraphTraverser = new TopologyGraphTraverser();
            sourceFinder = new TopologyGraphSourceFinder();
        }

        public long FindSource(long nodeGid)
        {
            ReaderWriterLockSlim graphLocker = topologyAnalysis.GetLock();

            graphLocker.EnterReadLock();

            TopologyGraphNode node = topologyAnalysis.GetNode(nodeGid);

            if (node == null)
            {
                graphLocker.ExitReadLock();

                Logger.Instance.Log($"[{GetType()}] Cannot find source for entity with gid: {nodeGid:X16}.");
                return 0;
            }

            long sourceGid = sourceFinder.FindSource(node);

            graphLocker.ExitReadLock();

            return sourceGid;
        }

        public IEnumerable<long> Read(long sourceGid)
        {
            List<long> nodesConnected = new List<long>();
            ReaderWriterLockSlim graphLocker = topologyAnalysis.GetLock();

            graphLocker.EnterReadLock();

            TopologyGraphNode source = topologyAnalysis.GetRoot(sourceGid);

            if (source == null)
            {
                Logger.Instance.Log($"[{GetType().Name}] Source node is null. Traversing the graph is not possible!");
                return nodesConnected;
            }

            topologyGraphTraverser.LoadRoot(source);

            TraverseGraph(topologyGraphTraverser, nodesConnected);

            graphLocker.ExitReadLock();

            return nodesConnected;
        }

        private void TraverseGraph(TopologyGraphTraverser graphTraverser, List<long> nodesConnected)
        {
            foreach (var node in graphTraverser)
            {
                nodesConnected.Add(node.Item);

                foreach (var shunt in node.Shunts)
                {
                    nodesConnected.Add(shunt.GlobalId);
                }
            }
        }
    }
}
