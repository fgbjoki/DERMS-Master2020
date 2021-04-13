using FieldSimulator.Commands.PowerSimulator.State;

namespace FieldSimulator.ViewModel
{
    public interface IPowerGridSimulatorViewModel
    {
        string FilePath { get; set; }

        void ChangeSimulatorState(PowerGridSimulatorState newState);
    }
}