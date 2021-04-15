using System;
using DERMS;
using FieldSimulator.Model;

namespace FieldSimulator.PowerSimulator.Model.Measurements
{
    public class DiscreteMeasurement : Measurement
    {
        public DiscreteMeasurement(long globalId) : base(globalId)
        {
        }

        public short Value { get; set; }

        public override void Update(DERMS.IdentifiedObject cimObject)
        {
            base.Update(cimObject);

            Discrete discreteCim = cimObject as Discrete;

            if (discreteCim == null)
            {
                return;
            }

            Value = (short)discreteCim.CurrentValue;
        }

        protected override RemotePointType ResolveRemotePointType(SignalDirection signalDirection)
        {
            switch (signalDirection)
            {
                case SignalDirection.Read:
                    return RemotePointType.DiscreteInput;
                case SignalDirection.ReadWrite:
                case SignalDirection.Write:
                    return RemotePointType.Coil;
                default:
                    throw new ArgumentException($"Discrete remote point cannot be defined with direction: {signalDirection}.");
            }
        }
    }
}
