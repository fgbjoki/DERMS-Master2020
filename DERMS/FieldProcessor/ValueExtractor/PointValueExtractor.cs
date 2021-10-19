using Common.Communication;
using Common.Logger;
using Common.SCADA;
using Common.ServiceInterfaces.NetworkDynamicsService;
using FieldProcessor.ModbusMessages;
using FieldProcessor.Model;
using FieldProcessor.RemotePointAddressCollector;
using System;
using System.Collections.Generic;
using System.Threading;

namespace FieldProcessor.ValueExtractor
{
    public class PointValueExtractor : IPointValueExtractor
    {
        private Dictionary<ModbusFunctionCode, ExtractValueProcessor> extractValueProcessors;
        private WCFClient<IFieldValuesProcessing> fieldValueProcessing;

        public PointValueExtractor(IRemotePointSortedAddressCollector remotePointAddressCollector)
        {
            InitializeProcessors(remotePointAddressCollector);
            fieldValueProcessing = new WCFClient<IFieldValuesProcessing>("valueExtractorEndpoint");
        }

        public void ExtractValues(ModbusMessageHeader request, ModbusMessageHeader response)
        {
            ExtractValueProcessor processor;

            if (!extractValueProcessors.TryGetValue(request.FunctionCode, out processor))
            {
                Logger.Instance.Log($"Cannot find extract value processor for function code {request.FunctionCode.ToString()}!");
                return;
            }

            ProcessFieldValues(processor.ExtractValues(request, response));  
        }
        private void ProcessFieldValues(IEnumerable<RemotePointFieldValue> fieldValues)
        {
            int retryTimesPolicy = 5;
            bool succesfullyProcessed = false;

            while (retryTimesPolicy > 0)
            {
                try
                {
                    fieldValueProcessing.Proxy.ProcessFieldValues(fieldValues);

                    succesfullyProcessed = true;
                    break;
                }
                catch (Exception e)
                {
                    retryTimesPolicy--;

                    Thread.Sleep(1000);
                }
            }

            if (!succesfullyProcessed)
            {
                throw new Exception("Values from field not processed");
            }
        }

        private void InitializeProcessors(IRemotePointSortedAddressCollector remotePointAddressCollector)
        {
            IFieldValueReader bitValueReader = new BitFieldValueReader();
            IFieldValueReader twoByteValueReader = new TwoByteFieldValueReader();
            IFieldValueReader fourByteValueReader = new FourByteFieldValueReader();

            extractValueProcessors = new Dictionary<ModbusFunctionCode, ExtractValueProcessor>()
            {
                { ModbusFunctionCode.ReadCoils, new ReadCommandExtractValueProcessor(bitValueReader, RemotePointType.Coil, remotePointAddressCollector) },
                { ModbusFunctionCode.ReadDiscreteInputs, new ReadCommandExtractValueProcessor(bitValueReader, RemotePointType.DiscreteInput, remotePointAddressCollector) },
                { ModbusFunctionCode.ReadHoldingRegisters, new AnalogReadCommandExtractValueProcessor(fourByteValueReader, RemotePointType.HoldingRegister, remotePointAddressCollector) },
                { ModbusFunctionCode.ReadInputRegisters, new AnalogReadCommandExtractValueProcessor(fourByteValueReader, RemotePointType.InputRegister, remotePointAddressCollector) },
                { ModbusFunctionCode.WriteSingleCoil, new SingleWriteCommandExtractValueProcessor(twoByteValueReader, RemotePointType.Coil, remotePointAddressCollector) },
                { ModbusFunctionCode.WriteSingleRegister, new SingleWriteCommandExtractValueProcessor(twoByteValueReader, RemotePointType.HoldingRegister, remotePointAddressCollector) },
                { ModbusFunctionCode.PresetMultipleRegisters, new MultipleWriteCommandExtractValueProcessor(fourByteValueReader, RemotePointType.HoldingRegister, remotePointAddressCollector) }
            };
        }
    }
}
