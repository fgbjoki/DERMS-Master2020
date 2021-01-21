using System;
using System.Threading;

namespace FieldProcessor.TCPCommunicationHandler
{
    public class FieldCommunicationHandler : IDisposable
    {
        private AutoResetEvent sendDone;

        private ICommunication client;

        private BlockingQueue<byte[]> requestQueue;

        private Thread sendingThread;

        private CancellationTokenSource tokenSource;

        public FieldCommunicationHandler(BlockingQueue<byte[]> requestQueue, ICommunication client, AutoResetEvent sendDone)
        {
            this.requestQueue = requestQueue;

            this.sendDone = sendDone;

            this.client = client;

            tokenSource = new CancellationTokenSource();

            sendingThread = new Thread(() => SendDataToClient(tokenSource.Token));

            client.StartClient();
            sendingThread.Start();
        }

        private void SendDataToClient(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                byte[] dataToSend = requestQueue.Dequeue();

                if (dataToSend == null)
                {
                    continue;
                }

                client.Send(dataToSend);
                sendDone.WaitOne();
            }
        }

        public void Dispose()
        {
            // unblock
            requestQueue.Dispose();
            tokenSource.Cancel();
            sendDone.Set();
        }
    }
}
