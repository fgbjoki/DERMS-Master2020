using FieldProcessor;
using FieldProcessor.TCPCommunicationHandler;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UnitTests.SCADA.FieldCommunicationHandlerTests
{
    class FieldCommunicationHandlerTests
    {
        private ICommunication client;
        private AutoResetEvent sendDone;
        private ModbusMessageArbitrator arbitrator;

        [SetUp]
        public void SetUp()
        {
            BlockingQueue<byte[]> responseQueue = new BlockingQueue<byte[]>();
            arbitrator = new ModbusMessageArbitrator(responseQueue);
            sendDone = new AutoResetEvent(false);
            client = Substitute.For<ICommunication>();
            
        }

        [Test]
        public void FieldCommunicatoinHandler_SendTest()
        {
            // Assign
            BlockingQueue<byte[]> requestQueue = new BlockingQueue<byte[]>();
            FieldCommunicationHandler handler = new FieldCommunicationHandler(requestQueue, arbitrator, client, sendDone);
            byte[] sentData = new byte[] { 1, 2, 3, 4, 5 };

            client.When(x => x.Send(sentData))
                .Do(x => sendDone.Set());

            // Act
            requestQueue.Enqueue(sentData);

            // Assert
            client.Received().Send(sentData);

            // Clean up
            handler.Dispose();
        }
    }
}
