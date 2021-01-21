using FieldProcessor.ModbusMessages;
using FieldProcessor.Model;

namespace FieldProcessor.ValueExtractor
{
    public class ReadCommandExtractValueProcessor : ExtractValueProcessor
    {
        public ReadCommandExtractValueProcessor(IFieldValueReader fieldValueReader, RemotePointType remotePointType, IRemotePointAddressCollector remotePointAddressCollector) : base(fieldValueReader, remotePointType, remotePointAddressCollector)
        {
        }

        protected override ushort GetRequestedQuantity(ModbusMessageHeader request)
        {
            ModbusReadRequestMessage readRequest = request as ModbusReadRequestMessage;

            return readRequest.Quantity;
        }

        protected override ushort GetStartingAddress(ModbusMessageHeader request)
        {
            ModbusReadRequestMessage readRequest = request as ModbusReadRequestMessage;

            return readRequest.StartingAddress;
        }

        protected override byte[] GetFieldValues(ModbusMessageHeader response)
        {
            ModbusReadResponseMessage readResponse = response as ModbusReadResponseMessage;

            return readResponse.Values;
        }
    }
}
