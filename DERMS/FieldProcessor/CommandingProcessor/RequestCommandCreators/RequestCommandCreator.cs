using Common.SCADA.FieldProcessor;
using FieldProcessor.ModbusMessages;

namespace FieldProcessor.CommandingProcessor
{
    public abstract class RequestCommandCreator
    {
        protected RequestCommandCreator()
        {
        }

        public abstract ModbusMessageHeader CreateCommand(Command command);
    }
}
