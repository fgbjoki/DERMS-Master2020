using Common.SCADA.FieldProcessor;
using FieldProcessor.Model;
using Common.ComponentStorage;
using Common.Logger;

namespace FieldProcessor.CommandingProcessor
{
    public class SingleWriteCoilRequestCommandCreator : SingleWriteRequestCommandCreator
    {
        public SingleWriteCoilRequestCommandCreator(IStorage<RemotePoint> storage) : base(storage, ModbusFunctionCode.WriteSingleCoil)
        {
        }

        protected override byte[] GetCommandedValue(ChangeRemotePointValueCommand command, RemotePoint remotePoint)
        {
            if (command == null)
            {
                return null;
            }

            if (remotePoint == null)
            {
                Logger.Instance.Log($"Cannot find entity with gid: 0x{remotePoint.GlobalId:8X}.");
                return null;
            }

            if (remotePoint.Type != RemotePointType.Coil)
            {
                Logger.Instance.Log($"Cannot command {remotePoint.Type.ToString()} remote point type.");
                return null;
            }

            byte[] commandedValue = new byte[sizeof(ushort)];

            if (command.CommandingValue == 1)
            {
                commandedValue[0] = 0xFF;
                commandedValue[1] = 0x00;
            }
            else if (command.CommandingValue == 0)
            {
                commandedValue[0] = 0x00;
                commandedValue[1] = 0xFF;
            }
            else
            {
                Logger.Instance.Log($"Failed to create command for {typeof(SingleWriteCoilRequestCommandCreator).ToString()}, value {command.CommandingValue} cannot be stored in coil! Skipping command!");
                return null;
            }

            return commandedValue;
        }
    }
}
