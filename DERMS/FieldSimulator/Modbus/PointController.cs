using FieldSimulator.Modbus.SchemaAligner;
using FieldSimulator.Model;
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

        public PointController(ModbusSlave slave)
        {
            this.slave = slave;

            converter = new RegisterValueConverter();
        }

        public void Initialize()
        {
            InitializeEvents();
            InitializeRemotePoints();
        }

        public CoilWrapper[] Coils { get { return coils; } }

        public HoldingRegisterWrapper[] HoldingRegisters { get { return holdingRegisters; } }

        public InputRegisterWrapper[] InputRegisters { get { return inputRegisters; } }

        public DiscreteInputWrapper[] DiscreteInputs { get { return discreteInput; } }

        public SlaveRemotePoints GetSlaveRemotePoints()
        {
            return new SlaveRemotePoints()
            {
                Coils = Coils,
                HoldingRegisters = HoldingRegisters,
                InputRegisters = InputRegisters,
                DiscreteInput = DiscreteInputs
            };
        }

        /// <summary>
        /// UI to Slave data flow
        /// </summary>
        private void DiscretePointValueChanged(RemotePointType pointType, int index, short value)
        {
            switch (pointType)
            {
                case RemotePointType.Coil:
                    slave.Coils[index] = value == 1 ? true : false;
                    break;
                case RemotePointType.DiscreteInput:
                    slave.DiscreteInputs[index] = value == 1 ? true : false;
                    break;
            }
        }

        private void AnalogPointValueChanged(RemotePointType pointType, int index, float value)
        {
            Tuple<short, short> values = converter.SplitValue(value);
            switch (pointType)
            {
                case RemotePointType.HoldingRegister:
                    slave.HoldingRegisters[index + 1] = values.Item1;
                    slave.HoldingRegisters[index + 2] = values.Item2;
                    break;
                case RemotePointType.InputRegister:
                    slave.InputRegisters[index + 1] = values.Item1;
                    slave.InputRegisters[index + 2] = values.Item2;
                    break;
            }
        }

        private void InitializeEvents()
        {
            slave.CoilsChangedHandler += CoilsChangedHandler;
            slave.HoldingRegistersChangedHandler += HoldingRegisterChangedHandler;
        }

        /// <summary>
        /// Slave to UI data flow
        /// </summary>
        private void CoilsChangedHandler(int coil, int numberOfCoils)
        {
            for (int i = coil - 1; i < coil + numberOfCoils - 1; i++)
            {
                // - 1 because events is invoked with +1 offset, no idea why
                coils[i].Value = (short)(slave.Coils[i + 1] ? 1 : 0);
            }
        }

        /// <summary>
        /// Slave to UI data flow
        /// </summary>
        private void HoldingRegisterChangedHandler(int holdingRegister, int numberOfRegisters)
        {
            for (int i = holdingRegister - 1; i < numberOfRegisters + holdingRegister - 1; i += 2)
            {
                short value1 = slave.HoldingRegisters[i + 1];
                short value2 = slave.HoldingRegisters[i + 2];

                holdingRegisters[i].FloatValue = converter.ConvertToFloat(value1, value2);
            }
        }

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
                coils[i].DiscretePointValueChanged += DiscretePointValueChanged;

                holdingRegisters[i] = new HoldingRegisterWrapper(i*2);
                holdingRegisters[i].AnalogPointValueChanged += AnalogPointValueChanged;

                inputRegisters[i] = new InputRegisterWrapper(i*2);
                inputRegisters[i].AnalogPointValueChanged += AnalogPointValueChanged;

                discreteInput[i] = new DiscreteInputWrapper(i);
                discreteInput[i].DiscretePointValueChanged += DiscretePointValueChanged;
            }
        }
    }
}
