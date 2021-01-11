namespace FieldProcessor.TCPCommunicationHandler
{
    public interface ICommunication
    {
        void Send(byte[] data);
        bool StartClient();
    }
}