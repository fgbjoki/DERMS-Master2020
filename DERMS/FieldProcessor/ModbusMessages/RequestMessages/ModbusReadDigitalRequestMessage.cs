using FieldProcessor.Model;
using System;

namespace FieldProcessor.ModbusMessages
{
    public class ModbusReadDigitalRequestMessage : ModbusReadRequestMessage
    {
        public ModbusReadDigitalRequestMessage(ushort startingAddress, ushort quantity, ushort transactionIdentifier, ModbusFunctionCode functionCode) :
            base(startingAddress, quantity, transactionIdentifier, functionCode)
        {
            if (FunctionCode != ModbusFunctionCode.ReadCoils && FunctionCode != ModbusFunctionCode.ReadDiscreteInputs)
            {
                throw new ArgumentException($"Cannot create digital request with function code \'{FunctionCode.ToString()}\'.");
            }
        }

        protected override bool ValidateMessageSize(ModbusReadResponseMessage response)
        {
            return Math.Ceiling((double)Quantity / 8) == response.ByteCount;
        }
    }
}
