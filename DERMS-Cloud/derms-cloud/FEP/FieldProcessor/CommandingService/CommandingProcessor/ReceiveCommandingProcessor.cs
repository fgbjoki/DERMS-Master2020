using CommandingService.CommandingProcessor.RequestCommandCreators;
using Core.Common.AbstractModel;
using Core.Common.Communication.ServiceFabric.FEP;
using Core.Common.FEP.CommandingService;
using Core.Common.FEP.ModbusMessages;
using Core.Common.ServiceInterfaces.FEP.CommandingService;
using Core.Common.ServiceInterfaces.FEP.FEPStorage;
using Core.Common.ServiceInterfaces.FEP.MessageAggregator;
using System;
using System.Collections.Generic;

namespace CommandingService.CommandingProcessor
{
    public class ReceiveCommandingProcessor : ICommanding
    {
        private Dictionary<DMSType, RequestCommandCreator> commandCreators;

        private ICommandSender commandSender;

        private Action<string> logAction;

        public ReceiveCommandingProcessor(Action<string> logAction)
        {
            this.logAction = logAction;

            commandSender = new CommandSenderWCFClient();

            InitializeCommandCreators(new FEPStorageWCFClient());
        }

        public bool SendCommand(Command command)
        {
            DMSType dmsType = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(command.GlobalId);
            RequestCommandCreator commandCreator;
            if (!commandCreators.TryGetValue(dmsType, out commandCreator))
            {
                Log($"Cannot find command creator for DMSType: {dmsType.ToString()}. Skipping command.");
                return false;
            }

            ModbusMessageHeader modbusCommand = commandCreator.CreateCommand(command);

            if (modbusCommand == null)
            {
                Log($"Protocol specific command couldn't be created! Skipping command.");
                return false;
            }

            Log($"Successfully created protocol specific command!");

            return commandSender.SendCommand(modbusCommand);
        }

        private void InitializeCommandCreators(IFEPStorage fepStorage)
        {
            commandCreators = new Dictionary<DMSType, RequestCommandCreator>()
            {
                { DMSType.MEASUREMENTDISCRETE, new SingleWriteCoilRequestCommandCreator(fepStorage, Log) },
                { DMSType.MEASUREMENTANALOG, new WriteRegisterRequestCommandCreator(fepStorage, Log) }
            };
        }

        private void Log(string text)
        {
            logAction(text);
        }
    }
}
