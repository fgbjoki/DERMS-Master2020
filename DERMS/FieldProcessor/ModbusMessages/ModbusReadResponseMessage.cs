using FieldProcessor.Model;
using System;

namespace FieldProcessor.ModbusMessages
{
    ///                                         Read Response
    /// -------------------------------------------------------------------------------------------------
    /// ... | Byte Count | Values (1) | Values (2) | ... | Values (Byte Count - 1) | Valeus (Byte Count) |
    /// ... | 1 byte     | 1 byte     | 1 byte     | ... | 1 byte                  | 1 byte              |
    /// -------------------------------------------------------------------------------------------------
    
    public class ModbusReadResponseMessage : ModbusMessageHeader, IResponseMessage
    {
        private static readonly int byteCountOffset = 8;
        private static readonly int valuesOffset = 9;

        public ModbusReadResponseMessage() : base()
        {
        }

        public ModbusReadResponseMessage(byte[] values, ushort transactionIdentifier, ModbusFunctionCode functionCode) : base(transactionIdentifier, functionCode)
        {
            Values = values;
            ByteCount = (byte)values.Length;

            Length += sizeof(byte);

            if (Values != null)
            {
                Length += ByteCount;
            }
        }

        public byte ByteCount { get; private set; }
        public byte[] Values { get; private set; }

        public override void ConvertMessageFromBytes(byte[] rawData)
        {
            base.ConvertMessageFromBytes(rawData);
            ByteCount = rawData[byteCountOffset];

            Values = new byte[ByteCount];
            Buffer.BlockCopy(rawData, valuesOffset, Values, 0, ByteCount);
        }
    }
}
