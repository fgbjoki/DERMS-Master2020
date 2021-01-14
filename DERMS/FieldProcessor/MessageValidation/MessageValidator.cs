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

    public class MessageValidator : IDisposable
    {
        private Dictionary<ModbusFunctionCode, IResponseCommandCreator> responseCreators;

        private ModbusMessageHeader validationHeader;

        private Dictionary<ushort, ModbusMessageHeader> sentMessages;

        private BlockingQueue<byte[]> responseQueue;
        private BlockingQueue<byte[]> requestQueue;

        private Thread commandReceiveWorker;
        private CancellationTokenSource tokenSource;

        private IPointValueExtractor pointValueExtractor;

        public MessageValidator(BlockingQueue<byte[]> responseQueue, BlockingQueue<byte[]> requestQueue, IPointValueExtractor pointValueExtractor)
        {
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
            if (sentMessages.ContainsKey(command.TransactionIdentifier))
            {
                DERMSLogger.Instance.Log($"Command with transction identifier \'{command.TransactionIdentifier}\' already sent! Skipping further processing of this command.");
                commandSent = false;
            }
            else
            {
                sentMessages.Add(command.TransactionIdentifier, command);
                commandSent = requestQueue.Enqueue(command.TransfromMessageToBytes());
            }

            if (commandSent)
            {
                DERMSLogger.Instance.Log($"Command with transaction identifier \'{command.TransactionIdentifier}\' is now queued for data transmission.");
            }
            else if (sentMessages.ContainsKey(command.TransactionIdentifier))
            {
                sentMessages.Remove(command.TransactionIdentifier);
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
                { ModbusFunctionCode.WriteSingleRegister, writeResponseCreator }
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
                if (!sentMessages.TryGetValue(validationHeader.TransactionIdentifier, out requestCommand))
                {
                    DERMSLogger.Instance.Log($"Response message received with invalid \'transaction identifier\' ({validationHeader.TransactionIdentifier})");
                    continue;
                }

                sentMessages.Remove(validationHeader.TransactionIdentifier);

                ThreadPool.QueueUserWorkItem(ProcessCommand, new ProcessingParametersWrapper(requestCommand, receivedBytes));          
            }
        }

        private void ProcessCommand(object parameter)
        {
            ProcessingParametersWrapper parameters = (ProcessingParametersWrapper)parameter;

            IResponseCommandCreator commandCreator;
            if (!responseCreators.TryGetValue(validationHeader.FunctionCode, out commandCreator))
            {
                DERMSLogger.Instance.Log($"Non existent commanding processor with function code : \'{validationHeader.FunctionCode.ToString()}\'. Command will be skipped!");
                return;
            }

            ModbusMessageHeader responseCommand = commandCreator.CreateResponse(parameters.Request, parameters.ResponseBytes);

            // invalid command response
            if (responseCommand == null)
            {
                DERMSLogger.Instance.Log($"Invalid command with transaction id: {responseCommand.TransactionIdentifier} and function code {responseCommand.FunctionCode.ToString()}. Command will be skipped!");
                return;
            }

            ExtractValues(parameters.Request, responseCommand);
        }

        private void ExtractValues(ModbusMessageHeader request, ModbusMessageHeader response)
        {
            pointValueExtractor.ExtractValues(request, response);
        }
    }
}
