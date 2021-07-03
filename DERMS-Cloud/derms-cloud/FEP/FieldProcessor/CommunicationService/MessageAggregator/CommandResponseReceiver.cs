using Core.Common.ListenerDepedencyInjection;
using Core.Common.ServiceInterfaces.FEP.MessageAggregator;
using System;

namespace MessageAggregatorService.MessageAggregator
{
    public class CommandResponseReceiver : IResponseReceiver
    {
        private Action<string> log;

        private ObjectProxy<MessageValidator> responseReceiver;

        public CommandResponseReceiver(ObjectProxy<MessageValidator> responseReceiver, Action<string> log)
        {
            this.log = log;
            this.responseReceiver = responseReceiver;
        }
        public void ReceiveCommand(byte[] receivedBytes)
        {
            log("Command received");
            responseReceiver.Instance.ReceiveCommand(receivedBytes);
        }
    }
}
