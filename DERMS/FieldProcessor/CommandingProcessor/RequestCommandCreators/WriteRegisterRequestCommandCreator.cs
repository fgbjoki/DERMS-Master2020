using System;
using Common.SCADA.FieldProcessor;
using FieldProcessor.Model;
using Common.Logger;
using Common.ComponentStorage;
using FieldProcessor.ModbusMessages;

namespace FieldProcessor.CommandingProcessor
{
    public class WriteRegisterRequestCommandCreator : WriteRequestCommandCreator
    {
        public WriteRegisterRequestCommandCreator(IStorage<RemotePoint> storage) : base(storage, ModbusFunctionCode.WriteSingleRegister)
        {
        }

        protected override ModbusMessageHeader CreateProtocolCommand(ushort address, byte[] commandedValue)
        {
            return new ModbusPresetMultipleRegistersRequestMessage(address, 2, commandedValue, 0);
        }

        protected override byte[] GetCommandedValue(ChangeRemotePointValueCommand command, RemotePoint remotePoint)
        {
            if (command == null)
            {
                return null;
            }

            if (remotePoint == null)
            {
                Logger.Instance.Log($"Cannot find entity with gid: 0x{command.GlobalId:X16}.");
                return null;
            }

            if (remotePoint.Type != RemotePointType.HoldingRegister)
            {
                Logger.Instance.Log($"Cannot command {remotePoint.Type.ToString()} remote point type.");
                return null;
            }

            return BitConverter.GetBytes(command.CommandingValue);
        }
    }
}
