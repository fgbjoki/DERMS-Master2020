using System;
using FieldSimulator.PowerSimulator;
using FieldSimulator.ViewModel;

namespace FieldSimulator.Commands.PowerSimulator.State
{
    public class IdlePowerGridSimulatorState : PowerGridSimulatorState
    {
        public IdlePowerGridSimulatorState(IPowerGridSimulatorViewModel viewModel, IPowerSimulator simulator) : base(viewModel, simulator)
        {
        }

        public override bool CanLoadFile()
        {
            return true;
        }

        public override bool CanLoadSchema()
        {
            return false;
        }

        public override bool CanStartSimulation()
        {
            return false;
        }

        public override bool CanStopSimulation()
        {
            return false;
        }

        public override PowerGridSimulatorState LoadFile(string path)
        {
            viewModel.FilePath = path;
            return new LoadFilePowerGridSimulatorState(viewModel, simulator, path);
        }

        public override PowerGridSimulatorState LoadSchema()
        {
            return this;
        }

        public override PowerGridSimulatorState StartStopSimulator()
        {
            return this;
        }
    }
}
