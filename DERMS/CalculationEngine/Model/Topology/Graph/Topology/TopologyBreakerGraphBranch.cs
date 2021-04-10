namespace CalculationEngine.Model.Topology.Graph.Topology
{
    public class TopologyBreakerGraphBranch : TopologyGraphBranch
    {
        public TopologyBreakerGraphBranch(TopologyGraphNode parent, TopologyGraphNode child) : base(parent, child)
        {
        }

        public long BreakerGlobalId { get; set; }

        public BreakerState BreakerState { get; set; }

        public override bool DoesBranchConduct()
        {
            return BreakerState == BreakerState.CLOSED ? true : false;
        }
    }
}
