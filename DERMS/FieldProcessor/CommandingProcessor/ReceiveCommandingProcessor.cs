using Common.AbstractModel;
using Common.SCADA.FieldProcessor;
using System.Collections.Generic;
using FieldProcessor.MessageValidation;
using FieldProcessor.ModbusMessages;
using Common.ComponentStorage;
using Common.Logger;
using FieldProcessor.Model;

namespace FieldProcessor.CommandingProcessor
{
    public class ReceiveCommandingProcessor : ICommanding
    {
        private Dictionary<DMSType, RequestCommandCreator> commandCreators;

        private ICommandSender commandSender;

        public ReceiveCommandingProcessor(ICommandSender commandSender, IStorage<RemotePoint> discreteStorage, IStorage<RemotePoint> analogStorage)
        {
            this.commandSender = commandSender;

            InitializeCommandCreators(discreteStorage, analogStorage);
        }

        public bool SendCommand(Command command)
        {
            DMSType dmsType = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(command.GlobalId);
            RequestCommandCreator commandCreator;
            if (!commandCreators.TryGetValue(dmsType, out commandCreator))
            {
                Logger.Instance.Log($"Cannot find command creator for DMSType: {dmsType.ToString()}. Skipping command.");
                return false;
            }

            ModbusMessageHeader modbusCommand = commandCreator.CreateCommand(command);

            if (modbusCommand == null)
            {
                Logger.Instance.Log($"Protocol specific command couldn't be created! Skipping command.");
                return false;
            }

            Logger.Instance.Log($"Successfully created protocol specific command!");

            return commandSender.SendCommand(modbusCommand);
        }

        private void InitializeCommandCreators(IStorage<RemotePoint> discreteStorage, IStorage<RemotePoint> analogStorage)
        {
            commandCreators = new Dictionary<DMSType, RequestCommandCreator>()
            {
                { DMSType.MEASUREMENTDISCRETE, new SingleWriteCoilRequestCommandCreator(discreteStorage) },
                { DMSType.MEASUREMENTANALOG, new SingleWriteRegisterRequestCommandCreator(analogStorage) }
            };
        }
    }
}
