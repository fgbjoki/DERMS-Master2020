namespace NetworkDynamicsService.Model.RemotePoints
{
    public class AnalogRemotePoint : RemotePoint
    {
        public AnalogRemotePoint(long globalId, int address, float value, float minValue, float maxValue, AnalogRemotePointType remotePointType) : base(globalId, address)
        {
            CurrentValue = value;

            MinValue = minValue;
            MaxValue = maxValue;

            RemotePointType = remotePointType;
        }

        public float CurrentValue { get; set; }

        public float MinValue { get; set; }

        public float MaxValue { get; set; }

        public AnalogRemotePointType RemotePointType { get; set; }
    }
}
