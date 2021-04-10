using Common.ComponentStorage;

namespace CalculationEngine.Model.Topology.Transaction
{
    public class DiscreteRemotePoint : IdentifiedObject
    {
        public DiscreteRemotePoint(long globalId) : base(globalId)
        {
        }

        public long BreakerGid { get; set; }
        public int Value { get; set; }
    }
}
