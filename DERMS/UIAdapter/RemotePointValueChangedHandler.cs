using Messages;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIAdapter
{
    public class RemotePointValueChangedHandler : IHandleMessages<RemotePointValueChanged>
    {
        static ILog log = LogManager.GetLogger<RemotePointValueChangedHandler>();

        public Task Handle(RemotePointValueChanged message, IMessageHandlerContext context)
        {
            log.Info($"Received RemotePointValueChanged, GID = {message.GID}, Value = {message.Value}");

            return Task.CompletedTask;
        }
    }
}
