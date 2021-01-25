using Common.ComponentStorage;
using Common.SCADA.FieldProcessor;
using FieldProcessor.ModbusMessages;
using FieldProcessor.Model;

namespace FieldProcessor.CommandingProcessor
{
    public abstract class SingleWriteRequestCommandCreator : RequestCommandCreator
    {
        private ModbusFunctionCode functionCode;

        private IStorage<RemotePoint> storage;

        public SingleWriteRequestCommandCreator(IStorage<RemotePoint> storage, ModbusFunctionCode functionCode) : base()
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

            return new ModbusSingleWriteMessage(remotePoint.Address, commandedValue, 0, functionCode);
        }

        protected abstract byte[] GetCommandedValue(ChangeRemotePointValueCommand command, RemotePoint remotePoint);

        protected RemotePoint GetRemotePointByGid(long globalId)
        {
            return storage.GetEntity(globalId);
        }
    }
}
