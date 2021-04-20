using FieldSimulator.PowerSimulator.Model.Graph.Graphs;
using FieldSimulator.PowerSimulator.Model.Graph.Graphs.Branches;
using FieldSimulator.PowerSimulator.Model.Graph.Graphs.Nodes;
using FieldSimulator.PowerSimulator.Storage.Weather;
using System.Collections.Generic;
using System.Threading;

namespace FieldSimulator.PowerSimulator.Model.Graph.TopologyGraphCreator
{
    public class TopologyGraph : BaseMultipleRootGraph<TopologyGraphNode>
    {
        private Dictionary<long, List<TopologyBreakerGraphBranch>> breakerBranches;

        private ReaderWriterLockSlim locker;

        public TopologyGraph() : base()
        {
            locker = new ReaderWriterLockSlim();

            breakerBranches = new Dictionary<long, List<TopologyBreakerGraphBranch>>();
        }

        public void LoadBreakerBranches(IEnumerable<TopologyBreakerGraphBranch> breakerBranches)
        {
            foreach (var breakerBranch in breakerBranches)
            {
                List<TopologyBreakerGraphBranch> branches;
                if (!this.breakerBranches.TryGetValue(breakerBranch.BreakerGlobalId, out branches))
                {
                    branches = new List<TopologyBreakerGraphBranch>();
                    this.breakerBranches.Add(breakerBranch.BreakerGlobalId, branches);
                }

                branches.Add(breakerBranch);
            }
        }

        public List<TopologyBreakerGraphBranch> GetBreakerBranches(long breakerGid)
        {
            List<TopologyBreakerGraphBranch> branches;

            breakerBranches.TryGetValue(breakerGid, out branches);

            return branches;
        }

        public StorageLock StorageReaderLock
        {
            get
            {
                return new StorageLock(locker);
            }
        }

    }
}
