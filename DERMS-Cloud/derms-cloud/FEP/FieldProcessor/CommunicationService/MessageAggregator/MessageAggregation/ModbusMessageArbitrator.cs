using MessageAggregatorService.MessageAggregator.MessageAggregation;
using Microsoft.ServiceFabric.Data;
using System.Collections.Generic;

namespace FieldProcessor.TCPCommunicationHandler
{
    /// <summary>
    /// Unit used to store bytes received and extract modbus messages from provided bytes. If there is a valid message in given byte range
    /// the message will be sent to <see cref="BlockingQueue{T}"/> for further processing.
    /// </summary>
    public class ModbusMessageArbitrator
    {
        private readonly int bufferSize = 1024 * 80; // 80kb

        private readonly IReliableStateManager stateManager;

        private CircularMemoryBuffer buffer;

        private ModbusMessageExtractor analyzer;

        public ModbusMessageArbitrator(IReliableStateManager stateManager)
        {
            this.stateManager = stateManager;

            buffer = new CircularMemoryBuffer(bufferSize, stateManager);

            analyzer = new ModbusMessageExtractor(stateManager);
        }

        public List<byte[]> ReceiveData(byte[] data, int count)
        {
            buffer.Put(data, count);
            List<byte[]> newMessages = MessageRecongition();
            if (newMessages.Count == 0)
            {
                return null;
            }

            return newMessages;
        }

        private List<byte[]> MessageRecongition()
        {
            List<byte[]> messages = new List<byte[]>(1);

            byte[] message;

            while ((message = analyzer.ExtractMessage(buffer)) != null)
            {
                 messages.Add(message);
            }

            return messages;
        }
    }
}
