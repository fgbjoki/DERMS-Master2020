using System;
using FieldProcessor.Model;
using FieldProcessor.ExtensionMethods;

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

        public ushort TransactionIdentifier { get; private set; }
        public ushort ProtocolIdentifier { get; private set; } = 1; // MODBUS protocol
        public ushort Length { get; private set; }

        public byte UnitIdentifier { get; private set; } = 1; // We have only one RTU with id 1

        public ModbusFunctionCode FunctionCode { get; private set; }

        public virtual byte[] TransfromMessageToBytes()
        {
            byte[] byteArray;

            byteArray = BitConverter.GetBytes(TransactionIdentifier).Append(BitConverter.GetBytes(ProtocolIdentifier)).Append(BitConverter.GetBytes(Length)).
                Append(UnitIdentifier).Append((byte)FunctionCode);

            return byteArray;
        }

        public virtual void ConvertMessageFromBytes(byte[] rawData)
        {
            TransactionIdentifier = BitConverter.ToUInt16(rawData, transactionIdentifierOffset);
            ProtocolIdentifier = BitConverter.ToUInt16(rawData, protocolIdentifierOffset);
            Length = BitConverter.ToUInt16(rawData, lengthOffset);
            UnitIdentifier = rawData[unitIdentifierOffset];
            FunctionCode = (ModbusFunctionCode)rawData[functionCodeOffeset];
        }
    }
}
