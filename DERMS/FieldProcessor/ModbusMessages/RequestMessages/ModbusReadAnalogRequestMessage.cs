using FieldProcessor.Model;
using System;

namespace FieldProcessor.ModbusMessages
{
    public class ModbusReadAnalogRequestMessage : ModbusReadRequestMessage
    {
        public ModbusReadAnalogRequestMessage(ushort startingAddress, ushort quantity, ushort transactionIdentifier, ModbusFunctionCode functionCode) : base(startingAddress, quantity, transactionIdentifier, functionCode)
        {
            if (FunctionCode != ModbusFunctionCode.ReadHoldingRegisters && FunctionCode != ModbusFunctionCode.ReadInputRegisters)
            {
                throw new ArgumentException($"Cannot create analog request with function code \'{FunctionCode.ToString()}\'.");
            }
        }

        protected override bool ValidateMessageSize(ModbusReadResponseMessage response)
        {
            return Quantity * 2 == response.ByteCount;
        }
    }
}
