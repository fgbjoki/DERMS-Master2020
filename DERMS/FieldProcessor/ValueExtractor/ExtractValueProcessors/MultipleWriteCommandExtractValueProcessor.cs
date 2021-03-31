using FieldProcessor.ModbusMessages;
using FieldProcessor.Model;
using FieldProcessor.RemotePointAddressCollector;

namespace FieldProcessor.ValueExtractor
{
    public class MultipleWriteCommandExtractValueProcessor : ExtractValueProcessor
    {
        public MultipleWriteCommandExtractValueProcessor(IFieldValueReader fieldValueReader, RemotePointType remotePointType, IRemotePointSortedAddressCollector remotePointAddressCollector) : base(fieldValueReader, remotePointType, remotePointAddressCollector)
        {
        }

        protected override byte[] GetFieldValues(ModbusMessageHeader request, ModbusMessageHeader response)
        {
            ModbusPresetMultipleRegistersRequestMessage requestMessage = request as ModbusPresetMultipleRegistersRequestMessage;

            return requestMessage.Values;
        }

        protected override ushort GetRequestedQuantity(ModbusMessageHeader request)
        {
            ModbusPresetMultipleRegistersRequestMessage requestMessage = request as ModbusPresetMultipleRegistersRequestMessage;

            return requestMessage.NumberOfRegisters;
        }

        protected override ushort GetStartingAddress(ModbusMessageHeader request)
        {
            ModbusPresetMultipleRegistersRequestMessage requestMessage = request as ModbusPresetMultipleRegistersRequestMessage;

            return requestMessage.StartingAddress;
        }

        protected override void MoveAddressCounter(ref int counter)
        {
            counter += 2;
        }
    }
}
