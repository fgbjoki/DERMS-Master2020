using System.Runtime.Serialization;

namespace Core.Common.FEP.ModbusMessages
{
    [DataContract]
    public enum ModbusFunctionCode : byte
    {
        [EnumMember]
        ReadCoils = 1,
        [EnumMember]
        ReadDiscreteInputs = 2,
        [EnumMember]
        ReadHoldingRegisters = 3,
        [EnumMember]
        ReadInputRegisters = 4,

        [EnumMember]
        WriteSingleCoil = 5,
        [EnumMember]
        WriteSingleRegister = 6,

        [EnumMember]
        PresetMultipleRegisters = 16
    }
}
