using Common.ComponentStorage;

namespace UIAdapter.Model.DERGroup
{
    public abstract class DistributedEnergyResource : IdentifiedObject
    {
        public DistributedEnergyResource(long globalId) : base(globalId)
        {
        }

        public virtual float ActivePower { get; set; }

        public float NominalPower { get; set; }
    }
}
