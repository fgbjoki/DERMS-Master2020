using Core.Common.Extensions;
using Core.Common.FEP.ModbusMessages.ResponseMessages;
using System;
using System.Runtime.Serialization;

namespace Core.Common.FEP.ModbusMessages.RequestMessages
{
    ///             Read Request
    /// ------------------------------------
    /// ... | Starting Address  | Quantity |
    /// ... | 2 bytes           | 2 bytes  |
    /// ------------------------------------

    [DataContract]
    public abstract class ModbusReadRequestMessage : ModbusMessageHeader
    {
        protected ModbusReadRequestMessage() : base()
        {

        }

        protected ModbusReadRequestMessage(ushort startingAddress, ushort quantity, ushort transactionIdentifier, ModbusFunctionCode functionCode) : base(transactionIdentifier, functionCode)
        {
            StartingAddress = startingAddress;
            Quantity = quantity;

            Length += sizeof(ushort) + sizeof(ushort);
        }

        [DataMember]
        public ushort StartingAddress { get; set; }
        [DataMember]
        public ushort Quantity { get; set; }

        public override byte[] TransfromMessageToBytes()
        {
            return base.TransfromMessageToBytes().
                Append(BitConverter.GetBytes(SwitchEndian(StartingAddress, false))).
                Append(BitConverter.GetBytes(SwitchEndian(Quantity, false)));
        }

        public override bool ValidateResponse(ModbusMessageHeader response)
        {
            ModbusReadResponseMessage readResponse = response as ModbusReadResponseMessage;

            return base.ValidateResponse(response) && ValidateMessageSize(readResponse);
        }

        protected abstract bool ValidateMessageSize(ModbusReadResponseMessage response);
    }
}
