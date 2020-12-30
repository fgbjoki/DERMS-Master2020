using System;
using FieldProcessor.ExtensionMethods;
using FieldProcessor.Model;

namespace FieldProcessor.ModbusMessages
{
    ///             Read Request
    /// ------------------------------------
    /// ... | Starting Address  | Quantity |
    /// ... | 2 bytes           | 2 bytes  |
    /// ------------------------------------
    
    public class ModbusReadRequestMessage : ModbusMessage, IRequestMessage
    {
        public ModbusReadRequestMessage() : base()
        {

        }

        public ModbusReadRequestMessage(ushort startingAddress, ushort quantity, ushort transactionIdentifier, ModbusFunctionCode functionCode) : base(transactionIdentifier, functionCode)
        {
            StartingAddress = startingAddress;
            Quantity = quantity;

            Length += sizeof(ushort) + sizeof(ushort);
        }

        public ushort StartingAddress { get; private set; }
        public ushort Quantity { get; private set; }

        public override byte[] TransfromMessageToBytes()
        {
            return base.TransfromMessageToBytes().
                Append(BitConverter.GetBytes(SwitchEndian(StartingAddress, false))).
                Append(BitConverter.GetBytes(SwitchEndian(Quantity, false)));
        }
    }
}
