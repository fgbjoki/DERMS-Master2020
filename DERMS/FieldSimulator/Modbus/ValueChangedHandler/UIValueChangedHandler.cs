using FieldSimulator.Modbus.SchemaAligner;
using FieldSimulator.Model;
using FieldSimulator.PowerSimulator.Storage;
using System;

namespace FieldSimulator.Modbus.ValueChangedHandler
{
    class UIValueChangedHandler : IValueChangedHandler
    {
        private IModbusSlave modbusSlave;
        private SimulatorRemotePoints simulatorRemotePoints;
        private RegisterValueConverter converter;
        private PowerGridSimulatorStorage powerGridSimulatorStorage;

        public UIValueChangedHandler(IModbusSlave modbusSlave, PowerGridSimulatorStorage powerGridSimulatorStorage, SimulatorRemotePoints simulatorRemotePoints, RegisterValueConverter converter)
        {
            this.modbusSlave = modbusSlave;
            this.simulatorRemotePoints = simulatorRemotePoints;
            this.converter = converter;
            this.powerGridSimulatorStorage = powerGridSimulatorStorage;

            InitializeHandler();
        }

        private void DiscretePointValueChanged(RemotePointType pointType, ushort address, short value)
        {
            switch (pointType)
            {
                case RemotePointType.Coil:
                    modbusSlave.Coils[address + 1] = value == 1 ? true : false;
                    break;
                case RemotePointType.DiscreteInput:
                    modbusSlave.DiscreteInputs[address + 1] = value == 1 ? true : false;
                    break;
            }

            powerGridSimulatorStorage.UpdateValue(pointType, address, value);
        }

        private void AnalogPointValueChanged(RemotePointType pointType, ushort address, float value)
        {
            Tuple<short, short> values = converter.SplitValue(value);
            switch (pointType)
            {
                case RemotePointType.HoldingRegister:
                    modbusSlave.HoldingRegisters[address + 1] = values.Item1;
                    modbusSlave.HoldingRegisters[address + 2] = values.Item2;
                    break;
                case RemotePointType.InputRegister:
                    modbusSlave.InputRegisters[address + 1] = values.Item1;
                    modbusSlave.InputRegisters[address + 2] = values.Item2;
                    break;
            }

            powerGridSimulatorStorage.UpdateValue(pointType, address, value);
        }

        private void InitializeHandler()
        {
            foreach (var coil in simulatorRemotePoints.Coils)
            {
                coil.DiscretePointValueChanged += DiscretePointValueChanged;
            }

            foreach (var discreteInput in simulatorRemotePoints.DiscreteInput)
            {
                discreteInput.DiscretePointValueChanged += DiscretePointValueChanged;
            }

            foreach (var holdingRegister in simulatorRemotePoints.HoldingRegisters)
            {
                holdingRegister.AnalogPointValueChanged += AnalogPointValueChanged;
            }

            foreach (var inputRegister in simulatorRemotePoints.InputRegisters)
            {
                inputRegister.AnalogPointValueChanged += AnalogPointValueChanged;
            }
        }

        public void Dispose()
        {
            foreach (var coil in simulatorRemotePoints.Coils)
            {
                coil.DiscretePointValueChanged -= DiscretePointValueChanged;
            }

            foreach (var discreteInput in simulatorRemotePoints.DiscreteInput)
            {
                discreteInput.DiscretePointValueChanged -= DiscretePointValueChanged;
            }

            foreach (var holdingRegister in simulatorRemotePoints.HoldingRegisters)
            {
                holdingRegister.AnalogPointValueChanged -= AnalogPointValueChanged;
            }

            foreach (var inputRegister in simulatorRemotePoints.InputRegisters)
            {
                inputRegister.AnalogPointValueChanged -= AnalogPointValueChanged;
            }
        }
    }
}
