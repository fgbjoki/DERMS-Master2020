using Core.Common.FEP.ModbusMessages;
using Core.Common.FEP.ModbusMessages.ResponseMessages;
using Core.Common.ReliableCollectionProxy;
using Core.Common.ServiceInterfaces.FEP.FieldCommunicator;
using Core.Common.ServiceInterfaces.FEP.FieldValueExtractor;
using Core.Common.ServiceInterfaces.FEP.MessageAggregator;
using Microsoft.ServiceFabric.Data;
using System;
using System.Collections.Generic;
using System.Threading;

namespace MessageAggregatorService.MessageAggregator
{
    public class MessageValidator : ICommandSender, IResponseReceiver
    {
        private readonly int lockerTimeout = 60000; // 60 seconds
        private readonly string sentMessagesDictionary = "sentMessages";

        private Dictionary<ModbusFunctionCode, IResponseCommandCreator> responseCreators;

        private ModbusMessageHeader validationHeader;

        private int currentTransactionIdentifier;

        private ReaderWriterLock locker;

        private IReliableStateManager stateManager;

        private Action<string> log;

        // TODO INITIALIZE
        private IFiledCommunicator fieldCommunicator;

        // TODO INITIALIZE
        private IFieldValueExtractor fieldValueExtractor;

        public MessageValidator(IReliableStateManager stateManager, Action<string> log)
        {
            this.log = log;

            locker = new ReaderWriterLock();

            InitializeCommandCreators();

            validationHeader = new ModbusMessageHeader();

            ReliableDictionaryProxy.CreateDictionary<ModbusMessageHeader, ushort>(stateManager, sentMessagesDictionary);
        }

        public bool SendCommand(ModbusMessageHeader command)
        {
            bool commandSent = true;

            if (command.TransactionIdentifier == 0)
            {
                GenerateTransactionIdentifier(command);
            }

            if (ReliableDictionaryProxy.EntityExists<ModbusMessageHeader, ushort>(stateManager, command.TransactionIdentifier, sentMessagesDictionary))
            {
                Log($"[{GetType().Name}] Command with transction identifier \'{command.TransactionIdentifier}\' already sent! Skipping further processing of this command.");
                commandSent = false;
            }
            else
            {
                ReliableDictionaryProxy.AddOrUpdateEntity(stateManager, command, command.TransactionIdentifier, sentMessagesDictionary);

                Log($"[{GetType().Name}] Sending command with transaction identifier \'{command.TransactionIdentifier}\'.");
                fieldCommunicator.Send(command.TransfromMessageToBytes());
            }

            if (commandSent)
            {
                Log($"[{GetType().Name}] Command with transaction identifier \'{command.TransactionIdentifier}\' is now queued for data transmission.");
            }
            else if (ReliableDictionaryProxy.EntityExists<ModbusMessageHeader, ushort>(stateManager, command.TransactionIdentifier, sentMessagesDictionary))
            {
                ReliableDictionaryProxy.Remove<ModbusMessageHeader, ushort>(stateManager, command.TransactionIdentifier, sentMessagesDictionary);
            }

            return commandSent;
        }

        public void ReceiveCommand(byte[] receivedBytes)
        {
            if (receivedBytes == null)
            {
                return;
            }

            validationHeader.ConvertMessageFromBytes(receivedBytes);

            ModbusMessageHeader requestCommand = ReliableDictionaryProxy.GetEntity<ModbusMessageHeader, ushort>(stateManager, validationHeader.TransactionIdentifier, sentMessagesDictionary);
            if (requestCommand == null)
            {
                Log($"[{GetType().Name}] Response message received with invalid \'transaction identifier\' ({validationHeader.TransactionIdentifier})");

                return;
            }

            ReliableDictionaryProxy.Remove<ModbusMessageHeader, ushort>(stateManager, validationHeader.TransactionIdentifier, sentMessagesDictionary);

            ProcessCommand(requestCommand, receivedBytes);
        }

        private void InitializeCommandCreators()
        {
            ResponseCommandCreator<ModbusReadResponseMessage> readResponseCreator = new ResponseCommandCreator<ModbusReadResponseMessage>();
            ResponseCommandCreator<ModbusSingleWriteMessage> writeResponseCreator = new ResponseCommandCreator<ModbusSingleWriteMessage>();

            responseCreators = new Dictionary<ModbusFunctionCode, IResponseCommandCreator>()
            {
                { ModbusFunctionCode.ReadCoils, readResponseCreator },
                { ModbusFunctionCode.ReadDiscreteInputs, readResponseCreator },
                { ModbusFunctionCode.ReadHoldingRegisters, readResponseCreator },
                { ModbusFunctionCode.ReadInputRegisters, readResponseCreator },
                { ModbusFunctionCode.WriteSingleCoil, writeResponseCreator },
                { ModbusFunctionCode.WriteSingleRegister, writeResponseCreator },
                { ModbusFunctionCode.PresetMultipleRegisters, new ResponseCommandCreator<ModbusPresetMultipleRegistersResponseMessage>() }
            };
        }

        private void ProcessCommand(ModbusMessageHeader request, byte[] responseBytes)
        {
            IResponseCommandCreator commandCreator;
            if (!responseCreators.TryGetValue(request.FunctionCode, out commandCreator))
            {
                Log($"[{GetType().Name}] Non existent commanding processor with function code : \'{request.FunctionCode}\'. Command will be skipped!");
                return;
            }

            ModbusMessageHeader responseCommand = null;
            try
            {
                responseCommand = commandCreator.CreateResponse(request, responseBytes);
            }
            catch (Exception e)
            {
                Log($"[{GetType().Name}] Couldn't convert message from raw bytes: {String.Join(",", responseBytes)}! Info:\n{e.Message}\nStack trace:\n{e.StackTrace}.");
            }

            // invalid command response
            if (responseCommand == null)
            {
                Log($"[{GetType().Name}] Invalid command with transaction id: {request.TransactionIdentifier} and function code {request.FunctionCode.ToString()}. Command will be skipped!");
                return;
            }

            ExtractValues(request, responseCommand);
        }

        private void ExtractValues(ModbusMessageHeader request, ModbusMessageHeader response)
        {
            Log($"[{GetType().Name}] Sending command with transaction id: {request.TransactionIdentifier} to value extractor.");
            fieldValueExtractor.ExtractValues(request, response);
        }

        private void GenerateTransactionIdentifier(ModbusMessageHeader command)
        {
            locker.AcquireWriterLock(lockerTimeout);

            if (currentTransactionIdentifier == ushort.MaxValue - 1)
            {
                currentTransactionIdentifier = 0;
            }

            locker.ReleaseWriterLock();

            Interlocked.Increment(ref currentTransactionIdentifier);

            command.TransactionIdentifier = (ushort)currentTransactionIdentifier;
        }

        private void Log(string text)
        {
            log(text);
        }
    }
}
