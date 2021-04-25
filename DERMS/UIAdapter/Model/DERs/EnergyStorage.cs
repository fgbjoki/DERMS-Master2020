using Common.ComponentStorage;

namespace UIAdapter.Model.DERs
{
    public class EnergyStorage : IdentifiedObject
    {
        public EnergyStorage(long globalId) : base(globalId)
        {
        }

        public float Capacity { get; set; }

        public float ActivePower { get; set; }

        public float NominalPower { get; set; }

        public float StateOfCharge { get; set; }

        public long GeneratorGid { get; set; }
    }
}
