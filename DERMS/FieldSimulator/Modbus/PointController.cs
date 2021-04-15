using FieldSimulator.Modbus.SchemaAligner;
using FieldSimulator.Modbus.ValueChangedHandler;
using FieldSimulator.Model;
using FieldSimulator.PowerSimulator.Storage;
using System;

namespace FieldSimulator.Modbus
{
    class PointController
    {
        private ModbusSlave slave;

        private CoilWrapper[] coils;
        private DiscreteInputWrapper[] discreteInput;
        private InputRegisterWrapper[] inputRegisters;
        private HoldingRegisterWrapper[] holdingRegisters;

        private RegisterValueConverter converter;

        private SimulatorRemotePoints slaveRemotePoints;

        private IValueChangedHandler slaveValueChangedHandler;
        private IValueChangedHandler simulatorValueChangedHandler;

        public PointController(ModbusSlave slave, PowerGridSimulatorStorage powerGridSimulatorStorage)
        {
            InitializeRemotePoints();

            this.slave = slave;

            converter = new RegisterValueConverter();

            slaveValueChangedHandler = new ModbusSlaveValueChangedHandler(slave, powerGridSimulatorStorage, SlaveRemotePoints, converter);
            simulatorValueChangedHandler = new UIValueChangedHandler(slave, powerGridSimulatorStorage, SlaveRemotePoints, converter);
        }

        public CoilWrapper[] Coils { get { return coils; } }

        public HoldingRegisterWrapper[] HoldingRegisters { get { return holdingRegisters; } }

        public InputRegisterWrapper[] InputRegisters { get { return inputRegisters; } }

        public DiscreteInputWrapper[] DiscreteInputs { get { return discreteInput; } }

        public SimulatorRemotePoints SlaveRemotePoints { get { return slaveRemotePoints; } }

        private void InitializeRemotePoints()
        {
            int maxIterations = 500;
            coils = new CoilWrapper[maxIterations];
            holdingRegisters = new HoldingRegisterWrapper[maxIterations];
            inputRegisters = new InputRegisterWrapper[maxIterations];
            discreteInput = new DiscreteInputWrapper[maxIterations];

            for (int i = 0; i < maxIterations; ++i)
            {
                coils[i] = new CoilWrapper(i);

                holdingRegisters[i] = new HoldingRegisterWrapper(i*2);

                inputRegisters[i] = new InputRegisterWrapper(i*2);

                discreteInput[i] = new DiscreteInputWrapper(i);
            }

            slaveRemotePoints = new SimulatorRemotePoints()
            {
                Coils = Coils,
                HoldingRegisters = HoldingRegisters,
                InputRegisters = InputRegisters,
                DiscreteInput = DiscreteInputs
            };

        }
    }
}
