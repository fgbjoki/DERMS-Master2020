using FieldProcessor.ModbusMessages;

namespace FieldProcessor.ValueExtractor
{
    public interface IPointValueExtractor
    {
        void ExtractValues(ModbusMessageHeader request, ModbusMessageHeader response);
    }
}
