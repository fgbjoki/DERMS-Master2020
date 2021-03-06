﻿using System;
using FieldSimulator.PowerSimulator;
using FieldSimulator.ViewModel;

namespace FieldSimulator.Commands.PowerSimulator.State
{
    public class StartSimulatorPowerGridSimulatorState : PowerGridSimulatorState
    {
        public StartSimulatorPowerGridSimulatorState(IPowerGridSimulatorViewModel viewModel, IPowerSimulator simulator) : base(viewModel, simulator)
        {
        }

        public override bool CanLoadFile()
        {
            return false;
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
            return true;
        }

        public override PowerGridSimulatorState LoadFile(string path)
        {
            return this;
        }

        public override PowerGridSimulatorState LoadSchema()
        {
            return this;
        }

        public override PowerGridSimulatorState StartStopSimulator()
        {
            simulator.Start();
            return new StopSimulatorPowerGridSimulatorState(viewModel, simulator);
        }
    }
}
