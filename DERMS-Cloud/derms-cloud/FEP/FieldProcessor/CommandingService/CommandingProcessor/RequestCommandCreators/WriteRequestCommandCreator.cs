using Core.Common.FEP.CommandingService;
using Core.Common.FEP.ModbusMessages;
using Core.Common.ServiceInterfaces.FEP.FEPStorage;
using Core.Common.Transaction.Models.FEP.FEPStorage;
using System;

namespace CommandingService.CommandingProcessor.RequestCommandCreators
{
    public abstract class WriteRequestCommandCreator : RequestCommandCreator
    {
        protected ModbusFunctionCode functionCode;

        private IFEPStorage fepStorage;

        public WriteRequestCommandCreator(IFEPStorage fepStorage, ModbusFunctionCode functionCode, Action<string> logAction) : base(logAction)
        {
            this.functionCode = functionCode;
            this.fepStorage = fepStorage;
        }

        public override ModbusMessageHeader CreateCommand(Command command)
        {
            RemotePoint remotePoint = GetRemotePointByGid(command.GlobalId);

            byte[] commandedValue = GetCommandedValue(command as ChangeRemotePointValueCommand, remotePoint);

            if (remotePoint == null || commandedValue == null)
            {
                return null;
            }

            return CreateProtocolCommand(remotePoint.Address, commandedValue);
        }

        protected abstract ModbusMessageHeader CreateProtocolCommand(ushort address, byte[] commandedValue);

        protected abstract byte[] GetCommandedValue(ChangeRemotePointValueCommand command, RemotePoint remotePoint);

        protected RemotePoint GetRemotePointByGid(long globalId)
        {
            return fepStorage.GetEntity(globalId);
        }
    }
}
