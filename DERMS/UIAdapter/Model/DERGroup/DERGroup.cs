namespace UIAdapter.Model.DERGroup
{
    public class DERGroup : DistributedEnergyResource
    {
        public DERGroup(long globalId) : base(globalId)
        {
        }

        public EnergyStorage EnergyStorage { get; set; }

        public Generator Generator { get; set; }

        public override float ActivePower
        {
            get
            {
                float totalActivePower = EnergyStorage.ActivePower;

                totalActivePower += Generator == null ? 0 : Generator.ActivePower;

                return totalActivePower;
            }
        }
    }
}
