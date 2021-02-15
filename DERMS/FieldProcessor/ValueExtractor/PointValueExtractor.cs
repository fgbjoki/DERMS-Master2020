using Common.Communication;
using Common.Logger;
using Common.SCADA;
using Common.ServiceInterfaces.NetworkDynamicsService;
using FieldProcessor.ModbusMessages;
using FieldProcessor.Model;
using FieldProcessor.RemotePointAddressCollector;
using System.Collections.Generic;

namespace FieldProcessor.ValueExtractor
{
    public class PointValueExtractor : IPointValueExtractor
    {
        private Dictionary<ModbusFunctionCode, ExtractValueProcessor> extractValueProcessors;
        private WCFClient<IFieldValuesProcessing> fieldValueProcessing;

        public PointValueExtractor(IRemotePointSortedAddressCollector remotePointAddressCollector)
        {
            InitializeProcessors(remotePointAddressCollector);
            fieldValueProcessing = new WCFClient<IFieldValuesProcessing>();
        }

        public void ExtractValues(ModbusMessageHeader request, ModbusMessageHeader response)
        {
            ExtractValueProcessor processor;

            if (!extractValueProcessors.TryGetValue(request.FunctionCode, out processor))
            {
                Logger.Instance.Log($"Cannot find extract value processor for function code {request.FunctionCode.ToString()}!");
                return;
            }

            IEnumerable<RemotePointFieldValue> fieldValues = processor.ExtractValues(request, response);

            //fieldValueProcessing.Proxy.ProcessFieldValues(fieldValues);
        }

        private void InitializeProcessors(IRemotePointSortedAddressCollector remotePointAddressCollector)
        {
            IFieldValueReader bitValueReader = new BitFieldValueReader();
            IFieldValueReader twoByteValueReader = new TwoByteFieldValueReader();

            extractValueProcessors = new Dictionary<ModbusFunctionCode, ExtractValueProcessor>()
            {
                { ModbusFunctionCode.ReadCoils, new ReadCommandExtractValueProcessor(bitValueReader, RemotePointType.Coil, remotePointAddressCollector) },
                { ModbusFunctionCode.ReadDiscreteInputs, new ReadCommandExtractValueProcessor(bitValueReader, RemotePointType.DiscreteInput, remotePointAddressCollector) },
                { ModbusFunctionCode.ReadHoldingRegisters, new ReadCommandExtractValueProcessor(twoByteValueReader, RemotePointType.HoldingRegister, remotePointAddressCollector) },
                { ModbusFunctionCode.ReadInputRegisters, new ReadCommandExtractValueProcessor(twoByteValueReader, RemotePointType.InputRegister, remotePointAddressCollector) },
                { ModbusFunctionCode.WriteSingleCoil, new SingleWriteCommandExtractValueProcessor(twoByteValueReader, RemotePointType.Coil, remotePointAddressCollector) },
                { ModbusFunctionCode.WriteSingleRegister, new SingleWriteCommandExtractValueProcessor(twoByteValueReader, RemotePointType.HoldingRegister, remotePointAddressCollector) }
            };
        }
    }
}
