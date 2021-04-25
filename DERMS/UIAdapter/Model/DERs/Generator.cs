using Common.ComponentStorage;

namespace UIAdapter.Model.DERs
{
    public abstract class Generator : IdentifiedObject
    {
        public Generator(long globalId) : base(globalId)
        {
        }

        public float ActivePower { get; set; }

        public float NominalPower { get; set; }

        public long EnergyStorageGid { get; set; }
    }
}
