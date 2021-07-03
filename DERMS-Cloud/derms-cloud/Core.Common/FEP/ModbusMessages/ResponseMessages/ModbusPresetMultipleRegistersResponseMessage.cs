using System;
using System.Runtime.Serialization;

namespace Core.Common.FEP.ModbusMessages.ResponseMessages
{
    [DataContract]
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

        [DataMember]
        public ushort StartingAddress { get; set; }

        [DataMember]
        public ushort NumberOfRegisters { get; set; }

        public override void ConvertMessageFromBytes(byte[] rawData)
        {
            base.ConvertMessageFromBytes(rawData);
            StartingAddress = SwitchEndian(BitConverter.ToUInt16(rawData, startingAddressOffset), true);
            NumberOfRegisters = SwitchEndian(BitConverter.ToUInt16(rawData, numberOfRegistersOffset), true);
        }
    }
}
