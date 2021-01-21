using FieldProcessor.ModbusMessages;

namespace FieldProcessor.MessageValidation
{
    public interface ICommandSender
    {
        bool SendCommand(ModbusMessageHeader command);
    }
}
