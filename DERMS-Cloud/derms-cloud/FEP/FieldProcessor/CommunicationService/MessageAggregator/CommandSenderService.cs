using Core.Common.FEP.ModbusMessages;
using Core.Common.ListenerDepedencyInjection;
using Core.Common.ServiceInterfaces.FEP.MessageAggregator;
using System;

namespace MessageAggregatorService.MessageAggregator
{
    public class CommandSenderService : ICommandSender
    {
        private Action<string> log;

        private ObjectProxy<MessageValidator> commandSender;

        public CommandSenderService(ObjectProxy<MessageValidator> commandSender, Action<string> log)
        {
            this.log = log;
            this.commandSender = commandSender;
        }

        public bool SendCommand(ModbusMessageHeader command)
        {
            log($"SendCommand started for {command.GetType().Name} with trasnction id: {command.TransactionIdentifier}");
            return commandSender.Instance.SendCommand(command);
        }
    }
}
