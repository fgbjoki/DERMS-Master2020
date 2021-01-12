using FieldProcessor.ModbusMessages;
using FieldProcessor.TCPCommunicationHandler;
using FieldProcessor.TCPCommunicationHandler.Collection;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace UnitTests.SCADA.TCPCommunicationHandler
{
    public class ModbusMessageExtractorTests
    {
        private readonly int bufferSize = 100;
        private readonly int modbusHeaderSize = 6;

        [Test]
        public void ModbusMessageExtractor_MessageToShortTest()
        {
            // Assign
            byte[] expectedMessage = null;
            CircularMemoryBuffer buffer = new CircularMemoryBuffer(bufferSize);
            ModbusMessageExtractor extractor = new ModbusMessageExtractor();

            ModbusReadRequestMessage requestMessage = new ModbusReadDigitalRequestMessage(0, 1, 1, FieldProcessor.Model.ModbusFunctionCode.ReadCoils);

            byte[] message = requestMessage.TransfromMessageToBytes();
            byte[] bufferMessage = new byte[message.Length - 1];

            Buffer.BlockCopy(message, 0, bufferMessage, 0, message.Length - 1);

            buffer.Put(bufferMessage);

            // Act
            byte[] extractedMessage = extractor.ExtractMessage(buffer);

            // Assert
            Assert.AreEqual(expectedMessage, extractedMessage);
        }

        [TestCaseSource("ModbusMessageExtractor_ReturnMessagesCases")]
        public void ModbusMessageExtractor_ReturnMessages(List<IRequestMessage> requestMessage)
        {
            // Assign
            List<byte[]> expectedMessages = new List<byte[]>(requestMessage.Count);

            CircularMemoryBuffer buffer = new CircularMemoryBuffer(bufferSize);
            ModbusMessageExtractor extractor = new ModbusMessageExtractor();

            foreach (IRequestMessage responseMessage in requestMessage)
            {
                byte[] bytes = responseMessage.TransfromMessageToBytes();
                expectedMessages.Add(bytes);
                buffer.Put(bytes);
            }

            List<byte[]> extractedMessages = new List<byte[]>(expectedMessages.Count);
            byte[] extractedMessage;

            // Act
            while ((extractedMessage = extractor.ExtractMessage(buffer)) != null)
            {
                extractedMessages.Add(extractedMessage);
            }

            // Assert
            CollectionAssert.AreEqual(expectedMessages, extractedMessages);
        }

        [Test]
        public void ModbusMessageExtractor_HeaderTooShort()
        {
            // Assign
            byte[] expectedMessage = null;
            CircularMemoryBuffer buffer = new CircularMemoryBuffer(bufferSize);
            ModbusMessageExtractor extractor = new ModbusMessageExtractor();

            byte[] message = new byte[modbusHeaderSize - 1];

            buffer.Put(message);

            // Act
            byte[] extractedMessage = extractor.ExtractMessage(buffer);

            // Assert
            Assert.AreEqual(expectedMessage, extractedMessage);
        }

        private static object[] ModbusMessageExtractor_ReturnMessagesCases = new object[]
        {
            new object[] { new List<IRequestMessage>(1) { new ModbusSingleWriteMessage(1, 0, 1, FieldProcessor.Model.ModbusFunctionCode.WriteSingleCoil)} },
            new object[] { new List<IRequestMessage>(2) { new ModbusSingleWriteMessage(1, 0, 1, FieldProcessor.Model.ModbusFunctionCode.WriteSingleCoil), new ModbusSingleWriteMessage(2, 0, 2, FieldProcessor.Model.ModbusFunctionCode.WriteSingleRegister) } }
        };
    }
}
