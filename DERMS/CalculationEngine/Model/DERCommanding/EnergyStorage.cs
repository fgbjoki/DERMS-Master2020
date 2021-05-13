namespace CalculationEngine.Model.DERCommanding
{
    public class EnergyStorage : DistributedEnergyResource
    {
        public EnergyStorage(long globalId) : base(globalId)
        {
        }

        public float Capacity { get; set; }

        public float StateOfCharge
        { get;
            set; }
        public long StateOfChargeMeasurementGid { get; set; }

        public long ActivePowerMeasurementGid { get; set; }
    }
}
