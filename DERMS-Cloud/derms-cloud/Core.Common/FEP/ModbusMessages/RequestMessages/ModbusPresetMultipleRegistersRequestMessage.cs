using Core.Common.Extensions;
using Core.Common.FEP.ModbusMessages.ResponseMessages;
using System;
using System.Runtime.Serialization;

namespace Core.Common.FEP.ModbusMessages.RequestMessages
{
    [DataContract]
    public class ModbusPresetMultipleRegistersRequestMessage : ModbusMessageHeader, IRequestMessage
    {
        public ModbusPresetMultipleRegistersRequestMessage() : base()
        {
        }

        public ModbusPresetMultipleRegistersRequestMessage(ushort startingAddress, ushort numberOfRegisters, byte[] values, ushort transactionIdentifier, ModbusFunctionCode functionCode = ModbusFunctionCode.PresetMultipleRegisters) 
            : base(transactionIdentifier, functionCode)
        {
            StartingAddress = startingAddress;
            NumberOfRegisters = numberOfRegisters;
            ByteCount = (byte)(values.Length);
            Values = values;
        }

        [DataMember]
        public ushort StartingAddress { get; set; }

        [DataMember]
        public ushort NumberOfRegisters { get; set; }

        [DataMember]
        public byte ByteCount { get; set; }

        [DataMember]
        public byte[] Values { get; set; }

        public override byte[] TransfromMessageToBytes()
        {
            byte[] rawMessage = base.TransfromMessageToBytes()
                .Append(BitConverter.GetBytes(SwitchEndian(StartingAddress, false)))
                .Append(BitConverter.GetBytes(SwitchEndian(NumberOfRegisters, false)))
                .Append(ByteCount)
                .Append(SwitchEndianOnValues());

            return rawMessage;
        }

        public override bool ValidateResponse(ModbusMessageHeader response)
        {
            ModbusPresetMultipleRegistersResponseMessage responseMessage = response as ModbusPresetMultipleRegistersResponseMessage;

            return base.ValidateResponse(response) && StartingAddress == responseMessage.StartingAddress && NumberOfRegisters == responseMessage.NumberOfRegisters;
        }

        private byte[] SwitchEndianOnValues()
        {
            byte[] copiedValues = new byte[Values.Length];

            for (int i = 0; i < Values.Length; i += 2)
            {
                copiedValues[i] = Values[i + 1];
                copiedValues[i + 1] = Values[i];
            }

            return copiedValues;
        }
    }
}
