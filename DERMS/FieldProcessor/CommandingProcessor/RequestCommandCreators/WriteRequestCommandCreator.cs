using Common.ComponentStorage;
using Common.SCADA.FieldProcessor;
using FieldProcessor.ModbusMessages;
using FieldProcessor.Model;

namespace FieldProcessor.CommandingProcessor
{
    public abstract class WriteRequestCommandCreator : RequestCommandCreator
    {
        protected ModbusFunctionCode functionCode;

        private IStorage<RemotePoint> storage;

        public WriteRequestCommandCreator(IStorage<RemotePoint> storage, ModbusFunctionCode functionCode) : base()
        {
            this.storage = storage;
            this.functionCode = functionCode;
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
            return storage.GetEntity(globalId);
        }
    }
}
