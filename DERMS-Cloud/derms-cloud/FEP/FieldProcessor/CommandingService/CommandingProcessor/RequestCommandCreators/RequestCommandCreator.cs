using Core.Common.FEP.CommandingService;
using Core.Common.FEP.ModbusMessages;
using System;

namespace CommandingService.CommandingProcessor.RequestCommandCreators
{
    public abstract class RequestCommandCreator
    {
        private Action<string> logAction;

        protected RequestCommandCreator(Action<string> logAction)
        {
            this.logAction = logAction;
        }

        public abstract ModbusMessageHeader CreateCommand(Command command);

        protected void Log(string text)
        {
            logAction(text);
        }
    }
}
