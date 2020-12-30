using System;
using FieldProcessor.Model;
using FieldProcessor.ExtensionMethods;
using System.Net;

namespace FieldProcessor.ModbusMessages
{                                           
    /// --------------------------------------------------------------------------------------------
    /// | Transaction Identifier | Protocol Identifier | Length  | Unit Identifier | Function code | ...
    /// | 2 bytes                | 2 bytes             | 2 bytes | 1 byte          |  1 byte       | ...
    /// --------------------------------------------------------------------------------------------

    public class ModbusMessage
    {
        private static readonly int transactionIdentifierOffset = 0;
        private static readonly int protocolIdentifierOffset = 2;
        private static readonly int lengthOffset = 4;
        private static readonly int unitIdentifierOffset = 6;
        private static readonly int functionCodeOffeset = 7;
        
        protected ModbusMessage()
        {

        }

        protected ModbusMessage(ushort transactionIdentifier, ModbusFunctionCode functionCode)
        {
            TransactionIdentifier = transactionIdentifier;
            FunctionCode = functionCode;
        }

        public ushort TransactionIdentifier { get; private set; }
        public ushort ProtocolIdentifier { get; private set; } = 0; // MODBUS protocol
        public ushort Length { get; protected set; } = 2; // function code + unit identifier

        public byte UnitIdentifier { get; private set; } = 1; // We have only one RTU with id 1

        public ModbusFunctionCode FunctionCode { get; private set; }

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
            Length =SwitchEndian(BitConverter.ToUInt16(rawData, lengthOffset), true);
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
    }
}
