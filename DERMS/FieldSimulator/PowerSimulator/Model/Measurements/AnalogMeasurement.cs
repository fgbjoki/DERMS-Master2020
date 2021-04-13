namespace FieldSimulator.PowerSimulator.Model.Measurements
{
    public enum MeasurementType
    {
        ActivePower,
        DeltaPower,
    }

    public enum AnalogRemotePointType
    {
        InputRegister,
        HoldingRegister
    }

    public class AnalogMeasurement : Measurement
    {
        public AnalogMeasurement(long globalId) : base(globalId)
        {
        }

        public MeasurementType MeasurementType { get; set; }
        public AnalogRemotePointType RemotePointType { get; set; }
    }
}
