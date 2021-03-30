using FieldProcessor.ModbusMessages;
using FieldProcessor.Model;
using FieldProcessor.RemotePointAddressCollector;

namespace FieldProcessor.ValueExtractor
{
    public class ReadCommandExtractValueProcessor : ExtractValueProcessor
    {
        public ReadCommandExtractValueProcessor(IFieldValueReader fieldValueReader, RemotePointType remotePointType, IRemotePointSortedAddressCollector remotePointAddressCollector) : base(fieldValueReader, remotePointType, remotePointAddressCollector)
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

        protected override byte[] GetFieldValues(ModbusMessageHeader request, ModbusMessageHeader response)
        {
            ModbusReadResponseMessage readResponse = response as ModbusReadResponseMessage;

            return readResponse.Values;
        }

        protected override void MoveAddressCounter(ref int counter)
        {
            counter++;
        }
    }
}
