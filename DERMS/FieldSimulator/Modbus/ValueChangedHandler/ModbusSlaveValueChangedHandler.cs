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
            int startingRegister = holdingRegister - 1;

            for (int i = 0; i < numberOfRegisters / 2; i++)
            {
                int currentRegisterAddress = startingRegister + i * 2;
                int currentSlaveIndex = currentRegisterAddress / 2;

                short value1 = modbusSlave.HoldingRegisters[currentRegisterAddress + 1];
                short value2 = modbusSlave.HoldingRegisters[currentRegisterAddress + 2];

                float newValue = converter.ConvertToFloat(value1, value2);
                slaveRemotePoints.HoldingRegisters[currentSlaveIndex].FloatValue = newValue;
                simulatorStorage.UpdateValue(RemotePointType.HoldingRegister, (ushort)(currentRegisterAddress), newValue);
            }
        }

        public void Dispose()
        {
            modbusSlave.CoilsChangedHandler -= CoilsChangedHandler;
            modbusSlave.HoldingRegistersChangedHandler -= HoldingRegisterChangedHandler;
        }
    }
}
