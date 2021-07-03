namespace Core.Common.FEP.ModbusMessages.RequestMessages
{
    public interface IRequestMessage
    {
        byte[] TransfromMessageToBytes();
        bool ValidateResponse(ModbusMessageHeader response);
    }
}
