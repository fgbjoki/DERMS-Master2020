using Core.Common.ServiceInterfaces.FEP.FieldCommunicator;
using Core.Common.ServiceInterfaces.FEP.MessageAggregator;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace FieldProcessor.TCPCommunicationHandler
{
    public class CommunicationState
    {
        // Client socket.  
        public Socket workSocket = null;
        // Size of receive buffer.  
        public const int BufferSize = 1024;
        // Receive buffer.  
        public byte[] buffer = new byte[BufferSize];
    }

    /// <summary>
    /// Uses TCP protocol to communicate with field units (RTUs).
    /// </summary>
    public class AsynchronousTCPClient : IFiledCommunicator
    {
        private AutoResetEvent connectionFailed;
        private AutoResetEvent connectionDone;

        private readonly Action<string> log;

        private int port;
        private readonly IResponseReceiver responseReceiver;
        private string ipAddress = string.Empty;

        private Socket client;

        private Thread tryConnectWorker;
        private Thread waitForConnectionWorker;

        public AsynchronousTCPClient(string ipAddress, int port, IResponseReceiver responseReceiver, Action<string> log)
        {
            this.log = log;
            this.port = port;
            this.ipAddress = ipAddress;
            this.responseReceiver = responseReceiver;

            connectionFailed = new AutoResetEvent(false);
            connectionDone = new AutoResetEvent(false);    
        }

        public void Initialize(CancellationToken cancellationToken)
        {
            tryConnectWorker = new Thread(() => TryConnect(cancellationToken));
            waitForConnectionWorker = new Thread(() => WaitForConnection(cancellationToken));

            tryConnectWorker.Start();
            waitForConnectionWorker.Start();
        }

        private void StartClient()
        {
            // Connect to a remote device.  
            try
            {
                IPAddress ipAddress = IPAddress.Parse(this.ipAddress);
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

                // Create a TCP/IP socket.  
                client = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                // Connect to the remote endpoint.  
                client.BeginConnect(remoteEP,
                    new AsyncCallback(ConnectCallback), client);
            
            }
            catch (Exception e)
            {
                Log(e.Message);
            }
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket client = (Socket)ar.AsyncState;

                // Complete the connection.  
                client.EndConnect(ar);

                connectionDone.Set();

                Receive(client);
            }
            catch (Exception e)
            {
                connectionFailed.Set();
                Log(e.Message);
            }
        }

        private void Receive(Socket client)
        {
            try
            {
                // Create the state object.  
                CommunicationState state = new CommunicationState();
                state.workSocket = client;

                // Begin receiving the data from the remote device.  
                client.BeginReceive(state.buffer, 0, CommunicationState.BufferSize, 0,
                    new AsyncCallback(ReceiveCallback), state);
            }
            catch (SocketException se)
            {
                Log(se.Message);
            }
            catch (Exception e)
            {
                Log(e.Message);
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the state object and the client socket
                // from the asynchronous state object.  
                CommunicationState state = (CommunicationState)ar.AsyncState;
                Socket client = state.workSocket;

                // Read data from the remote device.  
                int bytesRead = client.EndReceive(ar);

                if (bytesRead > 0)
                {
                    byte[] receivedBytes = new byte[bytesRead];
                    Buffer.BlockCopy(state.buffer, 0, receivedBytes, 0, bytesRead);
                    responseReceiver.ReceiveCommand(receivedBytes);

                    // Get the rest of the data.  
                    client.BeginReceive(state.buffer, 0, CommunicationState.BufferSize, 0,
                        new AsyncCallback(ReceiveCallback), state);
                }
            }
            catch (SocketException)
            {
                connectionFailed.Set();
            }
            catch (Exception e)
            {
                Log(e.Message);
            }
        }

        public void Send(byte[] data)
        {
            client.BeginSend(data, 0, data.Length, 0,
                new AsyncCallback(SendCallback), client);
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Complete sending the data to the remote device.  
                int bytesSent = client.EndSend(ar);
            }
            catch (SocketException se)
            {
                Log(se.Message);
                connectionFailed.Set();
            }
            catch (Exception e)
            {
                Log(e.Message);
            }
        }

        private void WaitForConnection(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                connectionDone.WaitOne();

                if (!token.IsCancellationRequested)
                {
                    Log($"[{this.GetType().Name}] Connection restored!");
                }
            }
        }

        private void TryConnect(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                StartClient();

                connectionFailed.WaitOne();
                Log($"[{this.GetType().Name}] Connection failed!");
            }
        }

        private void Log(string text)
        {
            log(text);
        }
    }
}
