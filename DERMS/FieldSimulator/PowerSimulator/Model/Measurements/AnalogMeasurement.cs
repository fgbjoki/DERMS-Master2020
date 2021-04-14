using System;

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

        public override void Update(DERMS.IdentifiedObject cimObject)
        {
            base.Update(cimObject);

            DERMS.Analog analogCim = cimObject as DERMS.Analog;

            if (analogCim == null)
            {
                return;
            }

            MeasurementType = MapCimMeasurementType(analogCim.MeasurementType);
            RemotePointType = MapRemotePointType(analogCim.Direction);
        }

        private MeasurementType MapCimMeasurementType(DERMS.MeasurementType cimMeasurementType)
        {
            switch (cimMeasurementType)
            {
                case DERMS.MeasurementType.ActivePower:
                    return MeasurementType.ActivePower;
                case DERMS.MeasurementType.DeltaPower:
                    return MeasurementType.DeltaPower;
                default:
                    throw new ArgumentException($"Cannot map cim measurement type: {cimMeasurementType}");
            }
        }

        private AnalogRemotePointType MapRemotePointType(DERMS.SignalDirection direction)
        {
            if (direction == DERMS.SignalDirection.Read)
            {
                return AnalogRemotePointType.InputRegister;
            }
            else
            {
                return AnalogRemotePointType.HoldingRegister;
            }
        }
    }
}
