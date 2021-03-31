namespace CalculationEngine.Model.Topology.Transaction
{
    public class Breaker : ConductingEquipment
    {
        public Breaker(long globalId, long discreteRemotePointGid) : base(globalId)
        {
            this.discreteRemotePointGid = discreteRemotePointGid;
        }

        public long discreteRemotePointGid { get; private set; }

        public BreakerState State { get; set; }
    }
}
