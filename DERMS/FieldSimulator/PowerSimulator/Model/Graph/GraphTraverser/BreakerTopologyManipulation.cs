using FieldSimulator.Model;
using FieldSimulator.PowerSimulator.Model.Graph.TopologyGraphCreator;
using FieldSimulator.PowerSimulator.Storage.Weather;
using System.Collections.Generic;

namespace FieldSimulator.PowerSimulator.Model.Graph.GraphTraverser
{
    public class BreakerTopologyManipulation
    {
        private TopologyGraph topologyGraph;
        private Dictionary<int, long> discreteBreakerMapping;
        private StorageLock storageLock;

        public BreakerTopologyManipulation()
        {
            discreteBreakerMapping = new Dictionary<int, long>();
        }

        public void AddBreakerMapping(ushort address, long breakerGid)
        {
            int hashCode = GetRemotePointHashCode(RemotePointType.Coil, address);

            discreteBreakerMapping.Add(hashCode, breakerGid);
        }

        public void LoadTopologyGraph(TopologyGraph topologyGraph)
        {
            this.topologyGraph = topologyGraph;
            storageLock = topologyGraph.StorageReaderLock;
        }

        public void ChangeBreakerState(ushort address, int rawValue)
        {
            int hashCode = GetRemotePointHashCode(RemotePointType.Coil, address);
            long breakerGid;

            if (!discreteBreakerMapping.TryGetValue(hashCode, out breakerGid))
            {
                return;
            }

            storageLock.EnterWriteLock();

            ChangeBreakerStates(breakerGid, rawValue);

            storageLock.ExitWriteLock();
        }

        private void ChangeBreakerStates(long breakerGid, int rawValue)
        {
            foreach (var breakerBranch in topologyGraph.GetBreakerBranches(breakerGid))
            {
                breakerBranch.BreakerState = rawValue == 0 ? Helpers.BreakerState.CLOSED : Helpers.BreakerState.OPEN;
            }
        }

        private int GetRemotePointHashCode(RemotePointType remotePointType, ushort address)
        {
            return (ushort)remotePointType | (address << sizeof(ushort) * 8);
        }
    }
}
