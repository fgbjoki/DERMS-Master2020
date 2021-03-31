namespace NetworkDynamicsService.Model.RemotePoints
{
    public class DiscreteRemotePoint : RemotePoint
    {
        public DiscreteRemotePoint(long globalId, int address, int currentValue, int normalValue, DiscreteRemotePointType remotePointType) : base(globalId, address)
        {
            CurrentValue = currentValue;
            NormalValue = normalValue;

            RemotePointType = remotePointType;

            DOMManipulations = 0;
        }

        public int NormalValue { get; set; }

        public int CurrentValue { get; set; }

        public int DOMManipulations { get; set; }

        public DiscreteRemotePointType RemotePointType { get; set; }
    }
}
