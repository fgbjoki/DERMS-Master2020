using System.Net.Sockets;

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
}
