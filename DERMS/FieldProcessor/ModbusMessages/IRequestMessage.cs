namespace FieldProcessor.ModbusMessages
{
    public interface IRequestMessage
    {
        byte[] TransfromMessageToBytes();
    }
}
