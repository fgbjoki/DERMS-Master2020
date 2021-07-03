using Core.Common.Transaction.Models.FEP.FEPStorage;
using Core.Common.FEP.ModbusMessages;
using Core.Common.FEP.CommandingService;
using Core.Common.ServiceInterfaces.FEP.FEPStorage;
using System;

namespace CommandingService.CommandingProcessor.RequestCommandCreators
{
    public class SingleWriteCoilRequestCommandCreator : WriteRequestCommandCreator
    {
        public SingleWriteCoilRequestCommandCreator(IFEPStorage fepStorage, Action<string> logAction) : base(fepStorage, ModbusFunctionCode.WriteSingleCoil, logAction)
        {
        }

        protected override ModbusMessageHeader CreateProtocolCommand(ushort address, byte[] commandedValue)
        {
            return new ModbusSingleWriteMessage(address, commandedValue, 0, functionCode);
        }

        protected override byte[] GetCommandedValue(ChangeRemotePointValueCommand command, RemotePoint remotePoint)
        {
            if (command == null)
            {
                return null;
            }

            if (remotePoint == null)
            {
                Log($"Cannot find entity with gid: 0x{remotePoint.GlobalId:X16}.");
                return null;
            }

            if (remotePoint.Type != RemotePointType.Coil)
            {
                Log($"Cannot command {remotePoint.Type.ToString()} remote point type.");
                return null;
            }

            byte[] commandedValue = new byte[sizeof(ushort)];

            if (command.CommandingValue == 1)
            {
                commandedValue[0] = 0x00;
                commandedValue[1] = 0xFF;
            }
            else if (command.CommandingValue == 0)
            {
                commandedValue[0] = 0x00;
                commandedValue[1] = 0x00;
            }
            else
            {
                Log($"Failed to create command for {typeof(SingleWriteCoilRequestCommandCreator).Name}, value {command.CommandingValue} cannot be stored in coil! Skipping command!");
                return null;
            }

            return commandedValue;
        }

        private void Log(string text)
        {

        }
    }
}
