using FieldSimulator.PowerSimulator;
using FieldSimulator.ViewModel;

namespace FieldSimulator.Commands.PowerSimulator.State
{
    public abstract class PowerGridSimulatorState 
    {
        protected IPowerGridSimulatorViewModel viewModel;
        protected IPowerSimulator simulator;

        public PowerGridSimulatorState(IPowerGridSimulatorViewModel viewModel, IPowerSimulator simulator)
        {
            this.viewModel = viewModel;
            this.simulator = simulator;
        }

        public abstract PowerGridSimulatorState LoadFile(string path);
        public abstract PowerGridSimulatorState StartStopSimulator();
        public abstract PowerGridSimulatorState LoadSchema();

        public abstract bool CanLoadFile();
        public abstract bool CanStartSimulation();
        public abstract bool CanStopSimulation();
        public abstract bool CanLoadSchema();
    }
}
