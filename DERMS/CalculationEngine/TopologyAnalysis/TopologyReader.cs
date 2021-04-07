using CalculationEngine.Model.Topology.Graph.Topology;
using Common.AbstractModel;
using Common.Logger;
using System.Collections.Generic;
using System.Threading;

namespace CalculationEngine.TopologyAnalysis
{
    public class TopologyReader : ITopologyReader
    {
        private ITopologyAnalysis topologyAnalysis;
        private TopologyGraphTraverser topologyGraphTraverser;

        private List<DMSType> dmsTypes;

        public TopologyReader(ITopologyAnalysis topologyAnalysis, List<DMSType> dmsTypes)
        {
            this.dmsTypes = dmsTypes;
            this.topologyAnalysis = topologyAnalysis;

            topologyGraphTraverser = new TopologyGraphTraverser();
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

            RemoveExcessEntities(nodesConnected);

            return nodesConnected;
        }

        private void RemoveExcessEntities(List<long> nodesConnected)
        {
            nodesConnected.RemoveAll(x => !dmsTypes.Contains((DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(x)));
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
