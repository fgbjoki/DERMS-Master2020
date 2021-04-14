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

        public DiscreteRemotePointType RemotePointType { get; set; }

        public override void Update(DERMS.IdentifiedObject cimObject)
        {
            base.Update(cimObject);

            DERMS.Discrete analogCim = cimObject as DERMS.Discrete;

            if (analogCim == null)
            {
                return;
            }

            RemotePointType = MapRemotePointType(analogCim.Direction);
        }

        private DiscreteRemotePointType MapRemotePointType(DERMS.SignalDirection direction)
        {
            if (direction == DERMS.SignalDirection.Read)
            {
                return DiscreteRemotePointType.DiscreteInput;
            }
            else
            {
                return DiscreteRemotePointType.Coil;
            }
        }
    }
}
