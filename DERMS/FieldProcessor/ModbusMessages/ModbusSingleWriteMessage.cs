using System;
using FieldProcessor.ExtensionMethods;
using FieldProcessor.Model;

namespace FieldProcessor.ModbusMessages
{
    public class ModbusSingleWriteMessage : ModbusMessage, IRequestMessage, IResponseMessage
    {
        private static readonly ushort remotePointAddressOffset = 8;
        private static readonly ushort remotePointValueOffset = 10;

        public ModbusSingleWriteMessage() : base()
        {

        }

        public ModbusSingleWriteMessage(ushort remotePointAddress, ushort remotePointValue, ushort transactionIdentifier, ModbusFunctionCode functionCode) : base(transactionIdentifier, functionCode)
        {
            RemotePointAddress = remotePointAddress;
            RemotePointValue = remotePointValue;

            Length += sizeof(ushort) + sizeof(ushort); // remote point address + remote point value
        }

        public ushort RemotePointAddress { get; private set; }
        public ushort RemotePointValue { get; set; }

        public override void ConvertMessageFromBytes(byte[] rawData)
        {
            base.ConvertMessageFromBytes(rawData);
            RemotePointAddress = SwitchEndian(BitConverter.ToUInt16(rawData, remotePointAddressOffset), true);
            RemotePointValue = SwitchEndian(BitConverter.ToUInt16(rawData, remotePointValueOffset), true);
        }

        public override byte[] TransfromMessageToBytes()
        {
            return base.TransfromMessageToBytes().
                Append(BitConverter.GetBytes(SwitchEndian(RemotePointAddress, false))).
                Append(BitConverter.GetBytes(SwitchEndian(RemotePointValue, false)));
        }
    }
}
