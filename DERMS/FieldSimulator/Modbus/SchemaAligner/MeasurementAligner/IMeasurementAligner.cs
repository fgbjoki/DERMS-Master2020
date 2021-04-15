using System.Collections.Generic;
using FieldSimulator.Model;
using FieldSimulator.PowerSimulator.Model.Measurements;

namespace FieldSimulator.Modbus.SchemaAligner.MeasurementAligner
{
    interface IMeasurementAligner
    {
        void AlignRemotePoints(Dictionary<int, BasePoint> slaveRemotePoints, IEnumerable<Measurement> remotePoints);
    }
}