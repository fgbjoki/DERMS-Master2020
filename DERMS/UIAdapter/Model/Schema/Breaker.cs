using Common.ComponentStorage;

namespace UIAdapter.Model.Schema
{
    public class Breaker : IdentifiedObject
    {
        public Breaker(long globalId) : base(globalId)
        {
        }

        public long MeasurementDiscreteGid { get; set; }

        public int CurrentValue { get; set; }
    }
}
