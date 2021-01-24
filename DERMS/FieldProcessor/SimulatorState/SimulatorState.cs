namespace FieldProcessor.SimulatorState
{
    public delegate void ConnectedDelegate();
    public delegate void DisconnectedDelegate();

    public class SimulatorStateNotifier : IConnectionNotifier, ISimulatorState
    {
        public void Connected()
        {
            ConnectedEvent.Invoke();
        }

        public void Disconnected()
        {
            DisconnectedEvent.Invoke();
        }

        public event ConnectedDelegate ConnectedEvent;

        public event DisconnectedDelegate DisconnectedEvent;
    }
}
