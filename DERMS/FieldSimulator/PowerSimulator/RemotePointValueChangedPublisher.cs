using FieldSimulator.Modbus.SchemaAligner;
using FieldSimulator.Model;

namespace FieldSimulator.PowerSimulator
{
    public class RemotePointValueChangedPublisher
    {
        private SimulatorRemotePoints simulatorRemotePoints;

        public RemotePointValueChangedPublisher(SimulatorRemotePoints simulatorRemotePoints)
        {
            this.simulatorRemotePoints = simulatorRemotePoints;
        }

        public void ChangeRemotePointValue(RemotePointType remotePointType, ushort address, float value)
        {
            switch (remotePointType)
            {
                case RemotePointType.HoldingRegister:
                    simulatorRemotePoints.HoldingRegisters[address / 2].FloatValue = value;
                    break;
                case RemotePointType.InputRegister:
                    simulatorRemotePoints.InputRegisters[address / 2].FloatValue = value;
                    break;
            }
        }

        public void ChangeRemotePointValue(RemotePointType remotePointType, ushort address, int value)
        {
            switch (remotePointType)
            {
                case RemotePointType.Coil:
                    simulatorRemotePoints.Coils[address].Value = (short)value;
                    break;
                case RemotePointType.DiscreteInput:
                    simulatorRemotePoints.DiscreteInput[address].Value = (short)value;
                    break;
            }
        }
    }
}
