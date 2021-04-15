using FieldSimulator.Modbus.SchemaAligner;
using FieldSimulator.Model;
using FieldSimulator.PowerSimulator.Storage;

namespace FieldSimulator.Modbus.ValueChangedHandler
{
    class ModbusSlaveValueChangedHandler : IValueChangedHandler
    {
        private IModbusSlave modbusSlave;
        private SimulatorRemotePoints slaveRemotePoints;
        private RegisterValueConverter converter;
        private PowerGridSimulatorStorage simulatorStorage;

        public ModbusSlaveValueChangedHandler(IModbusSlave modbusSlave, PowerGridSimulatorStorage simulatorStorage, SimulatorRemotePoints slaveRemotePoints, RegisterValueConverter converter)
        {
            this.simulatorStorage = simulatorStorage;
            this.modbusSlave = modbusSlave;
            this.slaveRemotePoints = slaveRemotePoints;
            this.converter = converter;

            modbusSlave.CoilsChangedHandler += CoilsChangedHandler;
            modbusSlave.HoldingRegistersChangedHandler += HoldingRegisterChangedHandler;
        }

        private void CoilsChangedHandler(int coil, int numberOfCoils)
        {
            for (int i = coil - 1; i < coil + numberOfCoils - 1; i++)
            {
                // - 1 because events is invoked with +1 offset, no idea why
                int newValue = modbusSlave.Coils[i + 1] ? 1 : 0;
                slaveRemotePoints.Coils[i].Value = (short)(newValue);
                simulatorStorage.UpdateValue(RemotePointType.Coil, (ushort)i, newValue);
            }
        }

        private void HoldingRegisterChangedHandler(int holdingRegister, int numberOfRegisters)
        {
            for (int i = holdingRegister - 1; i < numberOfRegisters + holdingRegister - 1; i += 2)
            {
                short value1 = modbusSlave.HoldingRegisters[i + 1];
                short value2 = modbusSlave.HoldingRegisters[i + 2];

                float newValue = converter.ConvertToFloat(value1, value2);
                slaveRemotePoints.HoldingRegisters[i].FloatValue = newValue;
                simulatorStorage.UpdateValue(RemotePointType.HoldingRegister, (ushort)i, newValue);
            }
        }

        public void Dispose()
        {
            modbusSlave.CoilsChangedHandler -= CoilsChangedHandler;
            modbusSlave.HoldingRegistersChangedHandler -= HoldingRegisterChangedHandler;
        }
    }
}
