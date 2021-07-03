using System;
using System.Net;
using System.Runtime.Serialization;
using Core.Common.Extensions;
using Core.Common.FEP.ModbusMessages.RequestMessages;

namespace Core.Common.FEP.ModbusMessages
{
    /// --------------------------------------------------------------------------------------------
    /// | Transaction Identifier | Protocol Identifier | Length  | Unit Identifier | Function code | ...
    /// | 2 bytes                | 2 bytes             | 2 bytes | 1 byte          |  1 byte       | ...
    /// --------------------------------------------------------------------------------------------

    [DataContract]
    public class ModbusMessageHeader : IRequestMessage
    {
        private static readonly int transactionIdentifierOffset = 0;
        private static readonly int protocolIdentifierOffset = 2;
        private static readonly int lengthOffset = 4;
        private static readonly int unitIdentifierOffset = 6;
        private static readonly int functionCodeOffeset = 7;
        
        public ModbusMessageHeader()
        {

        }

        public ModbusMessageHeader(ushort transactionIdentifier, ModbusFunctionCode functionCode)
        {
            TransactionIdentifier = transactionIdentifier;
            FunctionCode = functionCode;
        }

        [DataMember]
        public ushort TransactionIdentifier { get; set; }

        [DataMember]
        public ushort ProtocolIdentifier { get; set; } = 0; // MODBUS protocol

        [DataMember]
        public ushort Length { get; set; } = 2; // function code + unit identifier

        [DataMember]
        public byte UnitIdentifier { get; set; } = 1; // We have only one RTU with id 1

        [DataMember]
        public ModbusFunctionCode FunctionCode { get; set; }

        public virtual byte[] TransfromMessageToBytes()
        {
            byte[] byteArray;

            byteArray = BitConverter.GetBytes(SwitchEndian(TransactionIdentifier, false)).
                Append(BitConverter.GetBytes(SwitchEndian(ProtocolIdentifier, false))).
                Append(BitConverter.GetBytes(SwitchEndian(Length, false))).
                Append(UnitIdentifier).
                Append((byte)FunctionCode);

            return byteArray;
        }

        public virtual void ConvertMessageFromBytes(byte[] rawData)
        {
            TransactionIdentifier = SwitchEndian(BitConverter.ToUInt16(rawData, transactionIdentifierOffset), true);
            ProtocolIdentifier = SwitchEndian(BitConverter.ToUInt16(rawData, protocolIdentifierOffset), true);
            Length = SwitchEndian(BitConverter.ToUInt16(rawData, lengthOffset), true);
            UnitIdentifier = rawData[unitIdentifierOffset];
            FunctionCode = (ModbusFunctionCode)rawData[functionCodeOffeset];
        }

        protected ushort SwitchEndian(ushort value, bool fromNetwork)
        {
            if (fromNetwork)
            {
                return (ushort)IPAddress.NetworkToHostOrder((short)value);
            }
            else
            {
                return (ushort)IPAddress.HostToNetworkOrder((short)value);
            }
        }

        public virtual bool ValidateResponse(ModbusMessageHeader response)
        {
            return TransactionIdentifier == response.TransactionIdentifier && ProtocolIdentifier == response.ProtocolIdentifier &&
                UnitIdentifier == response.UnitIdentifier && FunctionCode == response.FunctionCode;
        }
    }
}
