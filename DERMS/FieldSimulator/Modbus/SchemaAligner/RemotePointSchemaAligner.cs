using System.Collections.Generic;
using System.Linq;
using FieldSimulator.PowerSimulator;
using FieldSimulator.Model;
using Common.AbstractModel;
using FieldSimulator.PowerSimulator.Model.Measurements;
using FieldSimulator.Modbus.SchemaAligner.MeasurementAligner;

namespace FieldSimulator.Modbus.SchemaAligner
{
    public class RemotePointSchemaAligner : IRemotePointSchemaModelAligner
    {
        private Dictionary<int, BasePoint> slaveRemotePoints;

        private Dictionary<DMSType, IMeasurementAligner> measurementAligners;

        public RemotePointSchemaAligner()
        {
            measurementAligners = new Dictionary<DMSType, IMeasurementAligner>()
            {
                { DMSType.MEASUREMENTANALOG, new AnalogMeasurementAligner() },
                { DMSType.MEASUREMENTDISCRETE, new DiscreteMeasurementAligner() }
            };
        }

        public void AlignRemotePoints(EntityStorage entityStorage)
        {
            AlignRemotePoints(DMSType.MEASUREMENTANALOG, entityStorage.Storage[DMSType.MEASUREMENTANALOG].Values.Cast<Measurement>());
            AlignRemotePoints(DMSType.MEASUREMENTDISCRETE, entityStorage.Storage[DMSType.MEASUREMENTDISCRETE].Values.Cast<Measurement>());
        }

        private void AlignRemotePoints(DMSType dmsType, IEnumerable<Measurement> remotePoints)
        {
            IMeasurementAligner measurementAligner = measurementAligners[dmsType];

            measurementAligner.AlignRemotePoints(slaveRemotePoints, remotePoints);
        }

        public void LoadSlaveRemotePoints(SlaveRemotePoints slaveRemotePoints)
        {
            this.slaveRemotePoints = new Dictionary<int, BasePoint>();

            LoadRemotePoints(slaveRemotePoints.Coils);
            LoadRemotePoints(slaveRemotePoints.DiscreteInput);
            LoadRemotePoints(slaveRemotePoints.HoldingRegisters);
            LoadRemotePoints(slaveRemotePoints.InputRegisters);
        }

        private int GetRemotePointHashCode(short pointType, short address)
        {
            return (ushort)pointType | (address << sizeof(short) * 8);
        }

        private void LoadRemotePoints(BasePoint[] remotePoints)
        {
            foreach (var remotePoint in remotePoints)
            {
                slaveRemotePoints.Add(remotePoint.GetHashCode(), remotePoint);
            }
        }
    }
}
