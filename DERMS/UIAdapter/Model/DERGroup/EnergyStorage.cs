namespace UIAdapter.Model.DERGroup
{
    public class EnergyStorage : DistributedEnergyResource
    {
        public EnergyStorage(long globalId) : base(globalId)
        {
        }

        public float StateOfCharge { get; set; }
    }
}
