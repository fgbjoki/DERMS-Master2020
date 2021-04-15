using EasyModbus;
using static EasyModbus.ModbusServer;

namespace FieldSimulator.Modbus
{
    class ModbusSlave : IModbusSlave
    {
        private ModbusServer server;

        public ModbusSlave(int port)
        {

            server = new ModbusServer();
            server.Port = port;
        }

        public bool ServerStarted { get; set; }

        public bool StartServer()
        {
            if (ServerStarted)
            {
                return false;
            }

            server.Listen();
            ServerStarted = true;

            return true;
        }

        public bool StopServer()
        {
            if (!ServerStarted)
            {
                return false;
            }

            server.StopListening();
            ServerStarted = false;

            return true;
        }

        public Coils Coils { get { return server.coils; } }
        public event CoilsChangedHandler CoilsChangedHandler
        {
            add { server.CoilsChanged += value; }
            remove { server.CoilsChanged -= value; }
        }

        public HoldingRegisters HoldingRegisters { get { return server.holdingRegisters; } }
        public event HoldingRegistersChangedHandler HoldingRegistersChangedHandler
        {
            add { server.HoldingRegistersChanged += value; }
            remove { server.HoldingRegistersChanged -= value; }
        }

        public InputRegisters InputRegisters { get { return server.inputRegisters; } }

        public DiscreteInputs DiscreteInputs { get { return server.discreteInputs; } }
    }
}
