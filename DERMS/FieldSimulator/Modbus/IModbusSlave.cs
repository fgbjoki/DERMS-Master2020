using EasyModbus;

namespace FieldSimulator.Modbus
{
    interface IModbusSlave
    {
        ModbusServer.Coils Coils { get; }
        ModbusServer.DiscreteInputs DiscreteInputs { get; }
        ModbusServer.HoldingRegisters HoldingRegisters { get; }
        ModbusServer.InputRegisters InputRegisters { get; }

        event ModbusServer.CoilsChangedHandler CoilsChangedHandler;
        event ModbusServer.HoldingRegistersChangedHandler HoldingRegistersChangedHandler;
    }
}