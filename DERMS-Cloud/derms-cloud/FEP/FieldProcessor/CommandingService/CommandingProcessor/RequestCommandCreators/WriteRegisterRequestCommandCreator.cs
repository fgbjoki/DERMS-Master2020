using System;
using Core.Common.Transaction.Models.FEP.FEPStorage;
using Core.Common.FEP.ModbusMessages;
using Core.Common.FEP.ModbusMessages.RequestMessages;
using Core.Common.FEP.CommandingService;
using Core.Common.ServiceInterfaces.FEP.FEPStorage;

namespace CommandingService.CommandingProcessor.RequestCommandCreators
{
    public class WriteRegisterRequestCommandCreator : WriteRequestCommandCreator
    {
        public WriteRegisterRequestCommandCreator(IFEPStorage fepStorage, Action<string> log) : base(fepStorage, ModbusFunctionCode.PresetMultipleRegisters, log)
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
                Log($"Cannot find entity with gid: 0x{command.GlobalId:X16}.");
                return null;
            }

            if (remotePoint.Type != RemotePointType.HoldingRegister)
            {
                Log($"Cannot command {remotePoint.Type} remote point type.");
                return null;
            }

            return BitConverter.GetBytes(command.CommandingValue);
        }
    }
}
