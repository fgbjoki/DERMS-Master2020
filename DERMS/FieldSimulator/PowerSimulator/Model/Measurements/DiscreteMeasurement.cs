namespace FieldSimulator.PowerSimulator.Model.Measurements
{
    public enum DiscreteRemotePointType
    {
        Coil,
        DiscreteInput
    }

    public class DiscreteMeasurement : Measurement
    {
        public DiscreteMeasurement(long globalId) : base(globalId)
        {
        }

        public DiscreteRemotePointType DiscreteRemotePointType { get; set; }
    }
}
