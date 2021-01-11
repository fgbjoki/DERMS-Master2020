using Common.Logger;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace FieldProcessor.TCPCommunicationHandler
{
    public class AsynchronousTCPClient : ICommunication
    {
        private AutoResetEvent connectDone;
        private AutoResetEvent sendDone;

        private ModbusMessageArbitrator arbitrator;

        private int port;
        private string ipAddress = string.Empty;

        private Socket client;

        public AsynchronousTCPClient(string ipAddress, int port, AutoResetEvent sendDone, ModbusMessageArbitrator arbitrator)
        {
            this.ipAddress = ipAddress;
            this.port = port;

            connectDone = new AutoResetEvent(false);
            this.sendDone = sendDone;

            this.arbitrator = arbitrator;
        }

        public bool StartClient()
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

                return connectDone.WaitOne();
            }
            catch (Exception e)
            {
                DERMSLogger.Instance.Log(e.Message);
            }

            return false;
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket client = (Socket)ar.AsyncState;

                // Complete the connection.  
                client.EndConnect(ar);

                Receive(client);
            }
            catch (Exception e)
            {
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
                    arbitrator.ReceiveData(state.buffer);
                    // Get the rest of the data.  
                    client.BeginReceive(state.buffer, 0, CommunicationState.BufferSize, 0,
                        new AsyncCallback(ReceiveCallback), state);
                }
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

                // Signal that all bytes have been sent.  
                sendDone.Set();
            }
            catch (Exception e)
            {
                DERMSLogger.Instance.Log(e.Message);
            }
        }
    }
}
