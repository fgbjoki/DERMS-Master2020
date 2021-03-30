using System;
using FieldProcessor.ModbusMessages;
using FieldProcessor.Model;
using FieldProcessor.RemotePointAddressCollector;

namespace FieldProcessor.ValueExtractor
{
    class SingleWriteCommandExtractValueProcessor : ExtractValueProcessor
    {
        public SingleWriteCommandExtractValueProcessor(IFieldValueReader fieldValueReader, RemotePointType remotePointType, IRemotePointSortedAddressCollector remotePointAddressCollector) : base(fieldValueReader, remotePointType, remotePointAddressCollector)
        {
        }

        protected override ushort GetRequestedQuantity(ModbusMessageHeader request)
        {
            return 1;
        }

        protected override ushort GetStartingAddress(ModbusMessageHeader request)
        {
            ModbusSingleWriteMessage writeRequest = request as ModbusSingleWriteMessage;

            return writeRequest.RemotePointAddress;
        }

        protected override byte[] GetFieldValues(ModbusMessageHeader request, ModbusMessageHeader response)
        {
            ModbusSingleWriteMessage writeResponse = response as ModbusSingleWriteMessage;

            return writeResponse.RemotePointValue;
        }

        protected override void MoveAddressCounter(ref int counter)
        {
            ++counter;
        }
    }
}
