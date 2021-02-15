using Common.Logger;
using Common.SCADA;
using FieldProcessor.ModbusMessages;
using FieldProcessor.Model;
using FieldProcessor.RemotePointAddressCollector;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FieldProcessor.ValueExtractor
{
    public abstract class ExtractValueProcessor
    {
        private IRemotePointSortedAddressCollector remotePointAddressCollector;

        protected IFieldValueReader fieldValueReader;
        protected RemotePointType remotePointType;

        public ExtractValueProcessor(IFieldValueReader fieldValueReader, RemotePointType remotePointType, IRemotePointSortedAddressCollector remotePointAddressCollector)
        {
            this.remotePointAddressCollector = remotePointAddressCollector;
            this.fieldValueReader = fieldValueReader;
            this.remotePointType = remotePointType;
        }

        public IEnumerable<RemotePointFieldValue> ExtractValues(ModbusMessageHeader request, ModbusMessageHeader response)
        {
            ushort startingAddress = GetStartingAddress(request);
            ushort quantity = GetRequestedQuantity(request);
            byte[] values = GetFieldValues(response);

            List<RemotePointFieldValue> remotePointFieldValues = new List<RemotePointFieldValue>(quantity);

            RemotePointType remotePointType;

            try
            {
                remotePointType = GetRemotePointTypeByFunctionCode(request.FunctionCode);
            }
            catch (Exception e)
            {
                Logger.Instance.Log($"[{GetType().ToString()}] {e.Message}.");
                return null;
            }

            List<RemotePoint> remotePoints = GetRemotePointsSortedByAddress(remotePointType);

            int startingIndex = FindStartingAddressIndex(remotePoints, startingAddress);
            int counter = 0;

            foreach (int fieldValue in fieldValueReader.CreateValueCollection(quantity, values))           
            {
                ushort value = (ushort)fieldValue;

                RemotePoint remotePoint = remotePoints[startingIndex + counter];

                if (remotePoint.Address != startingAddress + counter)
                {
                    Logger.Instance.Log($"Remote point with address {startingAddress + counter} of type {this.remotePointType.ToString()} does not exist!");
                    break;
                }

                RemotePointFieldValue remotePointFieldValue = new RemotePointFieldValue(remotePoint.GlobalId, value);

                remotePointFieldValues.Add(remotePointFieldValue);
                counter++;
            }

            return remotePointFieldValues;
        }

        protected abstract byte[] GetFieldValues(ModbusMessageHeader response);

        protected abstract ushort GetRequestedQuantity(ModbusMessageHeader request);

        protected abstract ushort GetStartingAddress(ModbusMessageHeader request);

        private int FindStartingAddressIndex(List<RemotePoint> remotePoints, ushort startingAddress)
        {
            for (int i = 0; i < remotePoints.Count; i++)
            {
                if (remotePoints[i].Address == startingAddress)
                {
                    return i;
                }
            }

            throw new Exception($"Remote point with address: {startingAddress} does not exists.");
        }

        private RemotePointType GetRemotePointTypeByFunctionCode(ModbusFunctionCode functionCode)
        {
            switch (functionCode)
            {
                case ModbusFunctionCode.ReadCoils:
                case ModbusFunctionCode.WriteSingleCoil:
                    return RemotePointType.Coil;
                case ModbusFunctionCode.ReadDiscreteInputs:
                    return RemotePointType.DiscreteInput;
                case ModbusFunctionCode.ReadHoldingRegisters:
                case ModbusFunctionCode.WriteSingleRegister:
                    return RemotePointType.HoldingRegister;
                case ModbusFunctionCode.ReadInputRegisters:
                    return RemotePointType.InputRegister;
                default:
                    throw new Exception($"Modbus function code {functionCode} does not exist!");
            }
        }

        /// <summary>
        /// Returns remote points sorted by address.
        /// </summary>
        private List<RemotePoint> GetRemotePointsSortedByAddress(RemotePointType remotePointType)
        {
            return remotePointAddressCollector.GetSortedAddresses(remotePointType);
        }

    }
}
