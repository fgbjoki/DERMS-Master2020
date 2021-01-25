using FieldProcessor;
using FieldProcessor.MessageValidation;
using FieldProcessor.ModbusMessages;
using FieldProcessor.Model;
using FieldProcessor.ValueExtractor;
using NSubstitute;
using NUnit.Framework;
using System.Threading;

namespace UnitTests.SCADA.MessageValidation
{
    [TestFixture]
    public class MessageValidationTests
    {
        private BlockingQueue<byte[]> requestQueue;
        private BlockingQueue<byte[]> responseQueue;
        private IPointValueExtractor pointValueExtractor;

        [SetUp]
        public void Setup()
        {
            requestQueue = new BlockingQueue<byte[]>();
            responseQueue = new BlockingQueue<byte[]>();
            pointValueExtractor = Substitute.For<IPointValueExtractor>();
        }

        [Test]
        public void MessageValidation_SendCommandSuccessfulTest()
        {
            ushort remotePointAddress = 0;
            ushort transactionIdentifier = 1;
            byte[] remotePointValue = new byte[sizeof(ushort)] { 0xFF, 0 };
            ModbusFunctionCode functionCode = ModbusFunctionCode.WriteSingleCoil;

            ModbusSingleWriteMessage request = new ModbusSingleWriteMessage(remotePointAddress, remotePointValue, transactionIdentifier, functionCode);

            MessageValidator messageValidator = new MessageValidator(responseQueue, requestQueue, pointValueExtractor);

            byte[] expectedBytes = request.TransfromMessageToBytes();

            // Act
            bool commandSent = messageValidator.SendCommand(request);
            byte[] queuedBytes = requestQueue.Dequeue();

            // Assert
            Assert.True(commandSent);
            CollectionAssert.AreEqual(expectedBytes, queuedBytes);
        }

        [Test]
        public void MessageValidation_SendCommandFailed_AleadyExistsTest()
        {
            ushort remotePointAddress = 0;
            ushort transactionIdentifier = 1;
            byte[] remotePointValue = new byte[sizeof(ushort)] { 0xFF, 0 };
            ModbusFunctionCode functionCode = ModbusFunctionCode.WriteSingleCoil;

            ModbusSingleWriteMessage request = new ModbusSingleWriteMessage(remotePointAddress, remotePointValue, transactionIdentifier, functionCode);

            MessageValidator messageValidator = new MessageValidator(responseQueue, requestQueue, pointValueExtractor);

            byte[] expectedBytes = request.TransfromMessageToBytes();

            // Act
            messageValidator.SendCommand(request);
            bool commandSent = messageValidator.SendCommand(request);

            // Assert
            Assert.False(commandSent);
        }

        [Test]
        public void MessageVlidation_ProcessCommandSuccessfulTest()
        {
            ushort remotePointAddress = 0;
            ushort transactionIdentifier = 1;
            byte[] remotePointValue = new byte[sizeof(ushort)] { 0xFF, 0 };
            ModbusFunctionCode functionCode = ModbusFunctionCode.WriteSingleCoil;

            ModbusSingleWriteMessage request = new ModbusSingleWriteMessage(remotePointAddress, remotePointValue, transactionIdentifier, functionCode);
            ModbusSingleWriteMessage response = new ModbusSingleWriteMessage(remotePointAddress, remotePointValue, transactionIdentifier, functionCode);

            MessageValidator messageValidator = new MessageValidator(responseQueue, requestQueue, pointValueExtractor);

            byte[] expectedBytes = request.TransfromMessageToBytes();

            // Act
            messageValidator.SendCommand(request);
            responseQueue.Enqueue(response.TransfromMessageToBytes());

            // Assert
            Assert.True(SpinWait.SpinUntil(SpinWaitTruePointValueExtractorCalled, 1000));
        }

        [Test]
        public void MessageVlidation_ProcessCommandUnSuccessfulTest()
        {
            ushort remotePointAddress = 0;
            ushort transactionIdentifier = 1;
            byte[] remotePointValue = new byte[sizeof(ushort)] { 0xFF, 0 };
            ModbusFunctionCode functionCode = ModbusFunctionCode.WriteSingleCoil;

            ModbusSingleWriteMessage request = new ModbusSingleWriteMessage(remotePointAddress, remotePointValue, transactionIdentifier, functionCode);
            ModbusSingleWriteMessage response = new ModbusSingleWriteMessage(remotePointAddress, remotePointValue, (ushort)(transactionIdentifier + 1), functionCode);

            MessageValidator messageValidator = new MessageValidator(responseQueue, requestQueue, pointValueExtractor);

            byte[] expectedBytes = request.TransfromMessageToBytes();

            // Act
            messageValidator.SendCommand(request);
            responseQueue.Enqueue(response.TransfromMessageToBytes());

            // Assert
            Assert.True(SpinWait.SpinUntil(SpinWaitFalsePointValueExtractorCalled, 1000));
        }

        private bool SpinWaitTruePointValueExtractorCalled()
        {
            try
            {
                pointValueExtractor.Received().ExtractValues(Arg.Compat.Any<ModbusMessageHeader>(), Arg.Compat.Any<ModbusMessageHeader>());
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool SpinWaitFalsePointValueExtractorCalled()
        {
            return !SpinWaitTruePointValueExtractorCalled();
        }
    }
}
