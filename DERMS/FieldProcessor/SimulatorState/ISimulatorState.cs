namespace FieldProcessor.SimulatorState
{
    public interface ISimulatorState
    {
        event ConnectedDelegate ConnectedEvent;
        event DisconnectedDelegate DisconnectedEvent;
    }
}