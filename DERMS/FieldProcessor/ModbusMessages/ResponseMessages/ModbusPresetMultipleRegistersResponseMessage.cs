using System;
using FieldProcessor.Model;

namespace FieldProcessor.ModbusMessages
{
    public class ModbusPresetMultipleRegistersResponseMessage : ModbusMessageHeader
    {
        private readonly int startingAddressOffset = 8;
        private readonly int numberOfRegistersOffset = 10;

        public ModbusPresetMultipleRegistersResponseMessage()
        {
        }

        public ModbusPresetMultipleRegistersResponseMessage(ushort transactionIdentifier, ModbusFunctionCode functionCode) : base(transactionIdentifier, functionCode)
        {
        }

        public ushort StartingAddress { get; set; }

        public ushort NumberOfRegisters { get; set; }

        public override void ConvertMessageFromBytes(byte[] rawData)
        {
            base.ConvertMessageFromBytes(rawData);
            StartingAddress = SwitchEndian(BitConverter.ToUInt16(rawData, startingAddressOffset), true);
            NumberOfRegisters = SwitchEndian(BitConverter.ToUInt16(rawData, numberOfRegistersOffset), true);
        }
    }
}
