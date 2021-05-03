using Common.Logger;
using FieldProcessor.ModbusMessages;
using FieldProcessor.Model;
using FieldProcessor.ValueExtractor;
using System;
using System.Collections.Generic;
using System.Threading;

namespace FieldProcessor.MessageValidation
{
    class ProcessingParametersWrapper
    {
        public ProcessingParametersWrapper(ModbusMessageHeader request, byte[] responseBytes)
        {
            Request = request;
            ResponseBytes = responseBytes;
        }

        public ModbusMessageHeader Request { get; private set; }
        public byte[] ResponseBytes { get; private set; }
    }

    public class MessageValidator : ICommandSender,  IDisposable
    {
        private readonly int lockerTimeout = 10000; // 10 seconds

        private Dictionary<ModbusFunctionCode, IResponseCommandCreator> responseCreators;

        private ModbusMessageHeader validationHeader;

        private Dictionary<ushort, ModbusMessageHeader> sentMessages;

        private BlockingQueue<byte[]> responseQueue;
        private BlockingQueue<byte[]> requestQueue;

        private Thread commandReceiveWorker;
        private CancellationTokenSource tokenSource;

        private IPointValueExtractor pointValueExtractor;

        private int currentTransactionIdentifier;

        private ReaderWriterLock locker;

        public MessageValidator(BlockingQueue<byte[]> responseQueue, BlockingQueue<byte[]> requestQueue, IPointValueExtractor pointValueExtractor)
        {
            locker = new ReaderWriterLock();

            InitializeCommandCreators();

            validationHeader = new ModbusMessageHeader();

            sentMessages = new Dictionary<ushort, ModbusMessageHeader>(1);

            this.pointValueExtractor = pointValueExtractor;

            this.requestQueue = requestQueue;
            this.responseQueue = responseQueue;

            tokenSource = new CancellationTokenSource();

            commandReceiveWorker = new Thread(() => ReceiveCommands(tokenSource.Token));
            commandReceiveWorker.Start();
        }

        public bool SendCommand(ModbusMessageHeader command)
        {
            bool commandSent = true;

            if (command.TransactionIdentifier == 0)
            {
                GenerateTransactionIdentifier(command);
            }

            locker.AcquireReaderLock(lockerTimeout);

            if (sentMessages.ContainsKey(command.TransactionIdentifier))
            {
                Logger.Instance.Log($"Command with transction identifier \'{command.TransactionIdentifier}\' already sent! Skipping further processing of this command.");
                commandSent = false;

                locker.ReleaseReaderLock();
            }
            else
            {
                locker.ReleaseReaderLock();
                locker.AcquireWriterLock(lockerTimeout);

                sentMessages.Add(command.TransactionIdentifier, command);
                locker.ReleaseWriterLock();

                commandSent = requestQueue.Enqueue(command.TransfromMessageToBytes());
            }

            if (commandSent)
            {
                Logger.Instance.Log($"Command with transaction identifier \'{command.TransactionIdentifier}\' is now queued for data transmission.");
            }
            else if (sentMessages.ContainsKey(command.TransactionIdentifier))
            {
                locker.AcquireWriterLock(lockerTimeout);

                sentMessages.Remove(command.TransactionIdentifier);

                locker.ReleaseWriterLock();
            }

            return commandSent;
        }

        public void Dispose()
        {
            tokenSource.Cancel();
            responseQueue.Dispose();
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

        private void ReceiveCommands(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                byte[] receivedBytes = responseQueue.Dequeue();

                if (receivedBytes == null)
                {
                    continue;
                }

                validationHeader.ConvertMessageFromBytes(receivedBytes);

                ModbusMessageHeader requestCommand;

                locker.AcquireReaderLock(lockerTimeout);

                if (!sentMessages.TryGetValue(validationHeader.TransactionIdentifier, out requestCommand))
                {
                    Logger.Instance.Log($"Response message received with invalid \'transaction identifier\' ({validationHeader.TransactionIdentifier})");

                    locker.ReleaseReaderLock();

                    continue;
                }

                locker.ReleaseReaderLock();

                locker.AcquireWriterLock(lockerTimeout);

                sentMessages.Remove(validationHeader.TransactionIdentifier);

                locker.ReleaseWriterLock();

                ThreadPool.QueueUserWorkItem(ProcessCommand, new ProcessingParametersWrapper(requestCommand, receivedBytes));          
            }
        }

        private void ProcessCommand(object parameter)
        {
            ProcessingParametersWrapper parameters = (ProcessingParametersWrapper)parameter;

            IResponseCommandCreator commandCreator;
            if (!responseCreators.TryGetValue(parameters.Request.FunctionCode, out commandCreator))
            {
                Logger.Instance.Log($"Non existent commanding processor with function code : \'{parameters.Request.FunctionCode.ToString()}\'. Command will be skipped!");
                return;
            }

            ModbusMessageHeader responseCommand = null;
            try
            {
                responseCommand = commandCreator.CreateResponse(parameters.Request, parameters.ResponseBytes);
            }
            catch (Exception e)
            {
                Logger.Instance.Log($"[{GetType().Name}] Couldn't convert message from raw bytes: {String.Join(",", parameters.ResponseBytes)}! Info:\n{e.Message}\nStack trace:\n{e.StackTrace}.");
            }

            // invalid command response
            if (responseCommand == null)
            {
                Logger.Instance.Log($"Invalid command with transaction id: {parameters.Request.TransactionIdentifier} and function code {parameters.Request.FunctionCode.ToString()}. Command will be skipped!");
                return;
            }

            ExtractValues(parameters.Request, responseCommand);
        }

        private void ExtractValues(ModbusMessageHeader request, ModbusMessageHeader response)
        {
            pointValueExtractor.ExtractValues(request, response);
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
    }
}
