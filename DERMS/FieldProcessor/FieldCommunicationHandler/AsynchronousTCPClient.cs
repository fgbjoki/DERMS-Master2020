using Common.Logger;
using FieldProcessor.SimulatorState;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace FieldProcessor.TCPCommunicationHandler
{
    /// <summary>
    /// Uses TCP protocol to communicate with field units (RTUs).
    /// </summary>
    public class AsynchronousTCPClient : ICommunication
    {
        private AutoResetEvent connectionFailed;
        private AutoResetEvent connectionDone;
        private AutoResetEvent sendDone;

        private ModbusMessageArbitrator arbitrator;

        private int port;
        private string ipAddress = string.Empty;

        private IConnectionNotifier connectionStateNotifier;

        private Socket client;

        private Thread tryConnectWorker;
        private Thread waitForConnectionWorker;

        private CancellationTokenSource tokenSource;

        public AsynchronousTCPClient(string ipAddress, int port, AutoResetEvent sendDone, ModbusMessageArbitrator arbitrator, 
            IConnectionNotifier connectionStateNotifier)
        {
            this.ipAddress = ipAddress;
            this.port = port;

            connectionFailed = new AutoResetEvent(false);
            connectionDone = new AutoResetEvent(false);
            this.sendDone = sendDone;

            this.arbitrator = arbitrator;

            this.connectionStateNotifier = connectionStateNotifier;

            tokenSource = new CancellationTokenSource();

            tryConnectWorker = new Thread(() => TryConnect(tokenSource.Token));
            waitForConnectionWorker = new Thread(() => WaitForConnection(tokenSource.Token));

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
                DERMSLogger.Instance.Log(e.Message);
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
                DERMSLogger.Instance.Log(e.Message);
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
                connectionStateNotifier.Disconnected();
            }
            catch (Exception e)
            {
                DERMSLogger.Instance.Log(e.Message);
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
                    arbitrator.ReceiveData(state.buffer, bytesRead);
                    // Get the rest of the data.  
                    client.BeginReceive(state.buffer, 0, CommunicationState.BufferSize, 0,
                        new AsyncCallback(ReceiveCallback), state);
                }
            }
            catch (SocketException se)
            {
                connectionStateNotifier.Disconnected();
                connectionFailed.Set();
            }
            catch (Exception e)
            {
                DERMSLogger.Instance.Log(e.Message);
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
                connectionStateNotifier.Disconnected();
                connectionFailed.Set();
            }
            catch (Exception e)
            {
                DERMSLogger.Instance.Log(e.Message);
            }
            finally
            {
                sendDone.Set();
            }
        }

        private void WaitForConnection(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                connectionDone.WaitOne();

                if (!token.IsCancellationRequested)
                {
                    DERMSLogger.Instance.Log($"[{this.GetType().Name}] Connection restored!");
                    connectionStateNotifier.Connected();
                }
            }
        }

        private void TryConnect(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                StartClient();

                connectionFailed.WaitOne();
                DERMSLogger.Instance.Log($"[{this.GetType().Name}] Connection failed!");
            }
        }
    }
}
