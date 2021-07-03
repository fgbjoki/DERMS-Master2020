namespace Core.Common.FEP.ModbusMessages.ResponseMessages
{
    public interface IResponseMessage
    {
        void ConvertMessageFromBytes(byte[] rawData);
    }
}
