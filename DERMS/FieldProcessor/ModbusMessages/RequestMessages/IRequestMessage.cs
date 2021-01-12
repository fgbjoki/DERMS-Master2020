namespace FieldProcessor.ModbusMessages
{
    public interface IRequestMessage
    {
        byte[] TransfromMessageToBytes();
        bool ValidateResponse(ModbusMessageHeader response);
    }
}
