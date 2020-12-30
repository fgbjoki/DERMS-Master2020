namespace FieldProcessor.ModbusMessages
{
    public interface IResponseMessage
    {
        void ConvertMessageFromBytes(byte[] rawData);
    }
}
