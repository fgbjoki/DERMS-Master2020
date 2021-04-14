using FieldSimulator.Model;

namespace FieldSimulator.Modbus
{
    public delegate void InputRegisterValueChanged(int registerIndex, short value);

    class PointController
    {
        private ModbusSlave slave;

        private CoilWrapper[] coils;
        private DiscreteInputWrapper[] discreteInput;
        private InputRegisterWrapper[] inputRegisters;
        private HoldingRegisterWrapper[] holdingRegisters;

        public PointController(ModbusSlave slave)
        {
            this.slave = slave;
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

        /// <summary>
        /// UI to Slave data flow
        /// </summary>
        private void PointValueChanged(PointType pointType, int index, short value)
        {
            switch (pointType)
            {
                case PointType.Coil:
                    slave.Coils[index] = value == 1 ? true : false;
                    break;
                case PointType.DiscreteInput:
                    slave.DiscreteInputs[index] = value == 1 ? true : false;
                    break;
                case PointType.HoldingRegister:
                    slave.HoldingRegisters[index] = value;
                    break;
                case PointType.InputRegister:
                    slave.InputRegisters[index] = value;
                    break;
            }
        }

        private void AnalogPointValueChanged()

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
            for (int i = holdingRegister - 1; i < numberOfRegisters + holdingRegister - 1; i++)
            {
                // - 1 because events is invoked with +1 offset, no idea why
                int validationIndex = i % 2 == 0 ? i : i - 1;
                holdingRegisters[validationIndex].ChangeValue(slave.HoldingRegisters[i + 1], i);
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
                coils[i].PointValueChanged += PointValueChanged;

                holdingRegisters[i] = new HoldingRegisterWrapper(i*2);
                holdingRegisters[i].PointValueChanged += PointValueChanged;

                inputRegisters[i] = new InputRegisterWrapper(i*2);
                inputRegisters[i].PointValueChanged += PointValueChanged;

                discreteInput[i] = new DiscreteInputWrapper(i);
                discreteInput[i].PointValueChanged += PointValueChanged;
            }
        }
    }
}
