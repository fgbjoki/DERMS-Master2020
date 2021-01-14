using FieldProcessor.TCPCommunicationHandler.Collection;
using System;
using System.Collections.Generic;
using System.Threading;

namespace FieldProcessor.TCPCommunicationHandler
{
    /// <summary>
    /// Unit used to store bytes received and extract modbus messages from provided bytes. If there is a valid message in given byte range
    /// the message will be sent to <see cref="BlockingQueue{T}"/> for further processing.
    /// </summary>
    public class ModbusMessageArbitrator : IDisposable
    {
        private readonly int timeOutLock = 1000; // 1 second
        private readonly int bufferSize = 1024 * 20; // 20kb

        private CircularMemoryBuffer buffer;

        private BlockingQueue<byte[]> responseQueue;

        private AutoResetEvent messageReady;
        private ReaderWriterLock locker;

        private Thread messageRecognitionThread;

        private CancellationTokenSource cancellationTokenSource;

        private ModbusMessageExtractor analyzer;

        public ModbusMessageArbitrator(BlockingQueue<byte[]> responseQueue)
        {
            messageReady = new AutoResetEvent(false);
            locker = new ReaderWriterLock();

            buffer = new CircularMemoryBuffer(bufferSize);

            analyzer = new ModbusMessageExtractor();

            this.responseQueue = responseQueue;

            cancellationTokenSource = new CancellationTokenSource();

            messageRecognitionThread = new Thread(() => MessageRecongition(cancellationTokenSource.Token));
            messageRecognitionThread.Start();
        }

        public void ReceiveData(byte[] data)
        {
            locker.AcquireWriterLock(timeOutLock);
            buffer.Put(data);
            locker.ReleaseWriterLock();

            messageReady.Set();
        }

        private void MessageRecongition(CancellationToken cancelationToken)
        {
            List<byte[]> messages = new List<byte[]>(1);
            while (!cancelationToken.IsCancellationRequested)
            {
                messageReady.WaitOne();

                if (cancelationToken.IsCancellationRequested)
                {
                    return;
                }

                byte[] message;

                locker.AcquireWriterLock(timeOutLock);

                while ((message = analyzer.ExtractMessage(buffer)) != null)
                {
                    messages.Add(message);
                }

                locker.ReleaseWriterLock();

                foreach (byte[] messageToQueue in messages)
                {
                    responseQueue.Enqueue(messageToQueue);
                }

                if (messages.Count > 0)
                {
                    messages.Clear();
                }
            }
        }

        public void Dispose()
        {
            cancellationTokenSource.Cancel();
            messageReady.Set();

            if (locker.IsReaderLockHeld)
            {
                locker.ReleaseReaderLock();
            }

            if (locker.IsWriterLockHeld)
            {
                locker.ReleaseWriterLock();
            }
        }
    }
}
