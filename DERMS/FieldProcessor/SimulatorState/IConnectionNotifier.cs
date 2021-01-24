namespace FieldProcessor.SimulatorState
{
    public interface IConnectionNotifier
    {
        void Connected();
        void Disconnected();
    }
}
