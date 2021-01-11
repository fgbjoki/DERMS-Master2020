using FieldProcessor.TCPCommunicationHandler.Collection;
using System;
using System.Net;

namespace FieldProcessor.TCPCommunicationHandler
{
    public sealed class ModbusMessageExtractor
    {
        private readonly ushort headerSize = 6;
        private readonly int lengthOffset = 4;

        private ushort remainingBodyLength;

        private byte[] body;
        private byte[] header;
        private int offset;

        public ModbusMessageExtractor()
        {
            offset = 0;
        }

        public byte[] ExtractMessage(CircularMemoryBuffer buffer)
        {
            byte[] returnValue = null;

            if (ShouldSkipProcessing(buffer))
            {
                return null;
            }

            if (header == null)
            {
                ExtractHeader(buffer);
                body = new byte[remainingBodyLength];
                offset = 0;
            }

            while (remainingBodyLength > 0 && buffer.Size > 0)
            {
                ExtractBodyPart(buffer);
            }

            if (remainingBodyLength == 0)
            {
                returnValue = ConcatMessageParts();
                body = null;
                header = null;
            }

            return returnValue;
        }

        private void ExtractHeader(CircularMemoryBuffer buffer)
        {
            header = buffer.Get(headerSize);
            remainingBodyLength = (ushort)IPAddress.HostToNetworkOrder(BitConverter.ToInt16(header, lengthOffset));
        }

        private void ExtractBodyPart(CircularMemoryBuffer buffer)
        {
            int readAmount = 0;
            if (buffer.Size < remainingBodyLength)
            {
                readAmount = buffer.Size;
            }
            else
            {
                readAmount = remainingBodyLength;
            }

            remainingBodyLength -= (ushort)readAmount;

            byte[] partialMessage = buffer.Get(readAmount);

            Buffer.BlockCopy(partialMessage, 0, body, offset, readAmount);

            offset += readAmount;            
        }

        private bool ShouldSkipProcessing(CircularMemoryBuffer buffer)
        {
            return buffer.Size < headerSize && header == null;
        }

        private byte[] ConcatMessageParts()
        {
            byte[] completeMessage = new byte[headerSize + body.Length];
            Buffer.BlockCopy(header, 0, completeMessage, 0, headerSize);
            Buffer.BlockCopy(body, 0, completeMessage, headerSize, body.Length);

            return completeMessage;
        }
    }
}
