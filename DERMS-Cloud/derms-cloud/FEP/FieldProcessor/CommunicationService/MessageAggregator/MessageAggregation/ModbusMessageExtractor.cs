using Core.Common.ReliableCollectionProxy;
using MessageAggregatorService.MessageAggregator.MessageAggregation;
using Microsoft.ServiceFabric.Data;
using System;
using System.Net;

namespace FieldProcessor.TCPCommunicationHandler
{
    /// <summary>
    /// Class used to extract message from <see cref="CircularMemoryBuffer"/>. 
    /// </summary>
    public sealed class ModbusMessageExtractor
    {
        private readonly string bodyVariable = "body";
        private readonly string headerVariable = "header";
        private readonly string offsetVariable = "offset";
        private readonly string remainingBodyLengthVariable = "remainingBodyLength";

        private readonly ushort headerSize = 6;
        private readonly int lengthOffset = 4;

        private readonly IReliableStateManager stateManager;

        private ushort remainingBodyLength;

        public ModbusMessageExtractor(IReliableStateManager stateManager)
        {
            this.stateManager = stateManager;
            ReliableVariableProxy.AddVariable(stateManager, 0, offsetVariable);
        }

        public byte[] ExtractMessage(CircularMemoryBuffer buffer)
        {
            using (var tx = stateManager.CreateTransaction())
            {
                byte[] returnValue = null;

                if (ShouldSkipProcessing(buffer, tx))
                {
                    return null;
                }

                byte[] header = ReliableVariableProxy.GetVariable<byte[]>(stateManager, headerVariable);
                if (header == null)
                {
                    ExtractHeader(buffer, tx);
                    ReliableVariableProxy.SetVariable(stateManager, new byte[remainingBodyLength], bodyVariable);
                    ReliableVariableProxy.SetVariable(stateManager, 0, offsetVariable);
                }

                while (ReliableVariableProxy.GetVariable<ushort>(stateManager, remainingBodyLengthVariable) > 0 && buffer.Size > 0)
                {
                    ExtractBodyPart(buffer, tx);
                }

                if (remainingBodyLength == 0)
                {
                    returnValue = ConcatMessageParts(tx);
                    ReliableVariableProxy.SetVariable<byte[]>(stateManager, null, bodyVariable);
                    ReliableVariableProxy.SetVariable<byte[]>(stateManager, null, headerVariable);
                }

                tx.CommitAsync().GetAwaiter().GetResult();
                return returnValue;
            }
        }

        private void ExtractHeader(CircularMemoryBuffer buffer, ITransaction tx)
        {
            ReliableVariableProxy.SetVariable(stateManager, buffer.Get(headerSize), headerVariable, tx);
            byte[] header = ReliableVariableProxy.GetVariable<byte[]>(stateManager, headerVariable, tx);
            ushort remainingBodyLength = (ushort)IPAddress.HostToNetworkOrder(BitConverter.ToInt16(header, lengthOffset));
            ReliableVariableProxy.SetVariable(stateManager, remainingBodyLength, remainingBodyLengthVariable, tx);
        }

        private void ExtractBodyPart(CircularMemoryBuffer buffer, ITransaction tx)
        {
            int readAmount = 0;
            ushort remainingBodyLength = ReliableVariableProxy.GetVariable<ushort>(stateManager, remainingBodyLengthVariable, tx);
            if (buffer.Size < remainingBodyLength)
            {
                readAmount = buffer.Size;
            }
            else
            {
                readAmount = remainingBodyLength;
            }

            remainingBodyLength -= (ushort)readAmount;
            ReliableVariableProxy.SetVariable(stateManager, remainingBodyLength, remainingBodyLengthVariable, tx);

            byte[] partialMessage = buffer.Get(readAmount);

            int offset = ReliableVariableProxy.GetVariable<int>(stateManager, offsetVariable);
            byte[] body = ReliableVariableProxy.GetVariable<byte[]>(stateManager, bodyVariable);
            Buffer.BlockCopy(partialMessage, 0, body, offset, readAmount);
            ReliableVariableProxy.SetVariable(stateManager, body, bodyVariable);

            offset += readAmount;
            ReliableVariableProxy.SetVariable(stateManager, offset, offsetVariable);
        }

        /// <summary>
        /// Determines if processing of the message should be skipped. Processing will be skipped if 
        /// there are not enough bytes in <paramref name="buffer"/> for processing.
        /// </summary>
        private bool ShouldSkipProcessing(CircularMemoryBuffer buffer, ITransaction tx)
        {
            byte[] header = ReliableVariableProxy.GetVariable<byte[]>(stateManager, headerVariable, tx);

            return buffer.Size < headerSize && header == null;
        }

        private byte[] ConcatMessageParts(ITransaction tx)
        {
            byte[] header = ReliableVariableProxy.GetVariable<byte[]>(stateManager, headerVariable, tx);
            byte[] body = ReliableVariableProxy.GetVariable<byte[]>(stateManager, bodyVariable, tx);
            byte[] completeMessage = new byte[headerSize + body.Length];
            Buffer.BlockCopy(header, 0, completeMessage, 0, headerSize);
            Buffer.BlockCopy(body, 0, completeMessage, headerSize, body.Length);

            return completeMessage;
        }
    }
}
