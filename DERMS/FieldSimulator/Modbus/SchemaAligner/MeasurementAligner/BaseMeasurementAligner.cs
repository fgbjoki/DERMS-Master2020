using FieldSimulator.Model;
using FieldSimulator.PowerSimulator.Model.Measurements;
using System.Collections.Generic;

namespace FieldSimulator.Modbus.SchemaAligner.MeasurementAligner
{
    abstract class BaseMeasurementAligner<SlaveModelType> : IMeasurementAligner
        where SlaveModelType : BasePoint
    {
        public void AlignRemotePoints(Dictionary<int, BasePoint> slaveRemotePoints, IEnumerable<Measurement> remotePoints)
        {
            foreach (var remotePoint in remotePoints)
            {
                BasePoint slaveRemotePoint;
                if (!slaveRemotePoints.TryGetValue(GetRemotePointHashCode((short)remotePoint.RemotePointType, remotePoint.Address), out slaveRemotePoint))
                {
                    // log
                    continue;
                }

                SlaveModelType slaveModel = slaveRemotePoint as SlaveModelType;

                slaveRemotePoint.GlobalId = remotePoint.GlobalId;
                slaveRemotePoint.Name = remotePoint.Name;
                PopulateValue(slaveModel, remotePoint);
            }
        }

        protected abstract void PopulateValue(SlaveModelType slaveModel, Measurement cimModel);

        private int GetRemotePointHashCode(short pointType, short address)
        {
            return (ushort)pointType | (address << sizeof(short) * 8);
        }
    }
}
