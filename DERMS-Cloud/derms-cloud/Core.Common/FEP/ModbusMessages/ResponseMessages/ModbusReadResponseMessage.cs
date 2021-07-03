using System;
using System.Runtime.Serialization;

namespace Core.Common.FEP.ModbusMessages.ResponseMessages
{
    ///                                         Read Response
    /// -------------------------------------------------------------------------------------------------
    /// ... | Byte Count | Values (1) | Values (2) | ... | Values (Byte Count - 1) | Valeus (Byte Count) |
    /// ... | 1 byte     | 1 byte     | 1 byte     | ... | 1 byte                  | 1 byte              |
    /// -------------------------------------------------------------------------------------------------

    [DataContract]
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

        [DataMember]
        public byte ByteCount { get; set; }

        [DataMember]
        public byte[] Values { get; set; }

        public override void ConvertMessageFromBytes(byte[] rawData)
        {
            base.ConvertMessageFromBytes(rawData);
            ByteCount = rawData[byteCountOffset];

            Values = new byte[ByteCount];
            Buffer.BlockCopy(rawData, valuesOffset, Values, 0, ByteCount);
        }
    }
}
