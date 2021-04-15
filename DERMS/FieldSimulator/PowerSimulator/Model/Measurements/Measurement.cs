using FieldSimulator.Model;
using FieldSimulator.PowerSimulator.Model.Equipment;

namespace FieldSimulator.PowerSimulator.Model.Measurements
{
    public abstract class Measurement : IdentifiedObject
    {
        public Measurement(long globalId) : base(globalId)
        {
        }

        public string ConductingEquipmentID { get; set; }

        public ConductingEquipment ConductingEquipment { get; set; }

        public ushort Address { get; set; }

        public RemotePointType RemotePointType { get; set; }

        public override void Update(DERMS.IdentifiedObject cimObject)
        {
            base.Update(cimObject);

            DERMS.Measurement cimMeasurement = cimObject as DERMS.Measurement;

            if (cimMeasurement == null)
            {
                return;
            }

            ConductingEquipmentID = cimMeasurement.PowerSystemResource.ID;
            Address = (ushort)cimMeasurement.MeasurementAddress;
            RemotePointType = ResolveRemotePointType(cimMeasurement.Direction);
        }

        protected abstract RemotePointType ResolveRemotePointType(DERMS.SignalDirection signalDirection);
    }
}
