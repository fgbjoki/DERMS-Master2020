using FieldProcessor;
using FieldProcessor.ModbusMessages;
using FieldProcessor.TCPCommunicationHandler;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading;

namespace UnitTests.SCADA.TCPCommunicationHandler
{
    class ModbusMessageArbitratorTests
    {
        private readonly int spinWaitTime = 1000;

        [Test]
        public void ModbusMessageArbitrator_MessageToShort()
        {
            // Assign
            BlockingQueue<byte[]> queue = Substitute.For<BlockingQueue<byte[]>>();
            ModbusMessageArbitrator arbitrator = new ModbusMessageArbitrator(queue);

            ModbusReadRequestMessage requestMessage = new ModbusReadDigitalRequestMessage(0, 1, 1, FieldProcessor.Model.ModbusFunctionCode.ReadCoils);

            byte[] message = requestMessage.TransfromMessageToBytes();
            byte[] bufferMessage = new byte[message.Length - 1];

            Buffer.BlockCopy(message, 0, bufferMessage, 0, message.Length - 1);

            // Act
            arbitrator.ReceiveData(bufferMessage);

            // Assert
            SpinWait.SpinUntil(() => { return QueueSpinWait(queue); }, spinWaitTime);

            arbitrator.Dispose();
        }

        [Test]
        public void ModbusMessageArbitrator_ReceiveMessage()
        {
            // Assign
            BlockingQueue<byte[]> queue = Substitute.For<BlockingQueue<byte[]>>();
            ModbusMessageArbitrator arbitrator = new ModbusMessageArbitrator(queue);

            ModbusReadRequestMessage requestMessage = new ModbusReadDigitalRequestMessage(0, 1, 1, FieldProcessor.Model.ModbusFunctionCode.ReadCoils);

            byte[] expectedMessage = requestMessage.TransfromMessageToBytes().Concat(requestMessage.TransfromMessageToBytes()).ToArray();

            // Act
            arbitrator.ReceiveData(expectedMessage);

            // Assert
            SpinWait.SpinUntil(() => { return QueueSpinWait(queue, 2); }, spinWaitTime);

            arbitrator.Dispose();
        }

        private bool QueueSpinWait(BlockingQueue<byte[]> queue, int numberOfCalls = 1)
        {
            return queue.Received(numberOfCalls).Enqueue(Arg.Compat.Any<byte[]>());
        }
    }
}
