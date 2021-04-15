using FieldSimulator.ViewModel;
using FieldSimulator.PowerSimulator;
using CIM.Model;

namespace FieldSimulator.Commands.PowerSimulator.State
{
    public class LoadFilePowerGridSimulatorState : PowerGridSimulatorState
    {
        private string path;

        public LoadFilePowerGridSimulatorState(IPowerGridSimulatorViewModel viewModel, IPowerSimulator simulator, string path) : base(viewModel, simulator)
        {
            this.path = path;
        }

        public override bool CanLoadFile()
        {
            return true;
        }

        public override bool CanLoadSchema()
        {
            return true;
        }

        public override bool CanStartSimulation()
        {
            return false;
        }

        public override PowerGridSimulatorState LoadFile(string path)
        {
            this.path = path;
            viewModel.FilePath = path;
            return this;
        }

        public override PowerGridSimulatorState LoadSchema()
        {
            var cimModel = simulator.LoadSchema(path);
            var slaveModel = simulator.CreateSlaveModel(cimModel);
            simulator.LoadModel(slaveModel);

            return new LoadSchemaPowerGridSimulatorState(viewModel, simulator);
        }

        public override PowerGridSimulatorState StartSimulation()
        {
            return this;
        }
    }
}
