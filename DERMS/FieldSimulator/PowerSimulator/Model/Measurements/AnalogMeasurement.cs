using System;
using FieldSimulator.Model;
using DERMS;

namespace FieldSimulator.PowerSimulator.Model.Measurements
{
    public enum MeasurementType
    {
        ActivePower,
        DeltaPower,
        Percent
    }

    public class AnalogMeasurement : Measurement
    {
        public AnalogMeasurement(long globalId) : base(globalId)
        {
        }

        public MeasurementType MeasurementType { get; set; }

        public float Value { get; set; }

        public override void Update(DERMS.IdentifiedObject cimObject)
        {
            base.Update(cimObject);

            Analog analogCim = cimObject as Analog;

            if (analogCim == null)
            {
                return;
            }

            MeasurementType = MapCimMeasurementType(analogCim.MeasurementType);
            Value = analogCim.CurrentValue;
        }

        protected override RemotePointType ResolveRemotePointType(SignalDirection signalDirection)
        {
            switch (signalDirection)
            {
                case SignalDirection.Read:
                    return RemotePointType.InputRegister;
                case SignalDirection.ReadWrite:
                case SignalDirection.Write:
                    return RemotePointType.HoldingRegister;
                default:
                    throw new ArgumentException($"Analog remote point cannot be defined with direction: {signalDirection}");
            }
        }

        private MeasurementType MapCimMeasurementType(DERMS.MeasurementType cimMeasurementType)
        {
            switch (cimMeasurementType)
            {
                case DERMS.MeasurementType.ActivePower:
                    return MeasurementType.ActivePower;
                case DERMS.MeasurementType.Percent:
                    return MeasurementType.Percent;
                default:
                    throw new ArgumentException($"Cannot map cim measurement type: {cimMeasurementType}");
            }
        }
    }
}
