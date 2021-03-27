using Common.ComponentStorage;

namespace UIAdapter.Model.Schema
{
    public class EnergySource : IdentifiedObject
    {
        public EnergySource(long globalId) : base(globalId)
        {
        }

        public string SubstationName { get; set; }

        public long SubstationGid { get; set; }
    }
}
