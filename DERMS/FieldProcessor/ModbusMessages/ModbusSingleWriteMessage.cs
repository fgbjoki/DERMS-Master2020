using System;
using FieldProcessor.ExtensionMethods;
using FieldProcessor.Model;

namespace FieldProcessor.ModbusMessages
{
    public class ModbusSingleWriteMessage : ModbusMessageHeader, IResponseMessage
    {
        private static readonly ushort remotePointAddressOffset = 8;
        private static readonly ushort remotePointValueOffset = 10;

        public ModbusSingleWriteMessage() : base()
        {

        }

        public ModbusSingleWriteMessage(ushort remotePointAddress, byte[] remotePointValue, ushort transactionIdentifier, ModbusFunctionCode functionCode) : base(transactionIdentifier, functionCode)
        {
            RemotePointAddress = remotePointAddress;
            RemotePointValue = remotePointValue;

            Length += sizeof(ushort) + sizeof(ushort); // remote point address + remote point value
        }

        public ushort RemotePointAddress { get; private set; }
        public byte[] RemotePointValue { get; private set; }

        public override void ConvertMessageFromBytes(byte[] rawData)
        {
            base.ConvertMessageFromBytes(rawData);
            RemotePointAddress = SwitchEndian(BitConverter.ToUInt16(rawData, remotePointAddressOffset), true);

            RemotePointValue = new byte[sizeof(ushort)];
            Buffer.BlockCopy(rawData, remotePointValueOffset, RemotePointValue, 0, sizeof(ushort));
        }

        public override byte[] TransfromMessageToBytes()
        {
            return base.TransfromMessageToBytes().
                Append(BitConverter.GetBytes(SwitchEndian(RemotePointAddress, false))).
                Append(new byte[] { RemotePointValue[1], RemotePointValue[0] });
        }

        public override bool ValidateResponse(ModbusMessageHeader response)
        {
            ModbusSingleWriteMessage writeResponse = response as ModbusSingleWriteMessage;

            if (writeResponse == null)
            {
                return false;
            }

            return base.ValidateResponse(response) && RemotePointAddress == writeResponse.RemotePointAddress &&
                RemotePointValue[0] == writeResponse.RemotePointValue[1] && RemotePointValue[1] == writeResponse.RemotePointValue[0];
        }
    }
}
