namespace FieldProcessor.Model
{
    public enum ModbusFunctionCode : byte
    {
        ReadCoils = 1,
        ReadDiscreteInputs = 2,
        ReadHoldingRegisters = 3,
        ReadInputRegisters = 4,

        WriteSingleCoil = 5,
        WriteSingleRegister = 6
    }
}
