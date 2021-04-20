using FieldSimulator.Commands.PowerSimulator.State;
using FieldSimulator.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FieldSimulator.Commands.PowerSimulator
{
    class StopSimulationCommand : ICommand
    {
        private IPowerGridSimulatorViewModel viewModel;

        public StopSimulationCommand(IPowerGridSimulatorViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            if (parameter == null)
            {
                return false;
            }

            PowerGridSimulatorState simulatorState = (PowerGridSimulatorState)parameter;

            bool isSimulatorStarted = simulatorState is StartSimulatorPowerGridSimulatorState;

            return isSimulatorStarted && simulatorState.CanStopSimulation();
        }

        public void Execute(object parameter)
        {
            PowerGridSimulatorState simulatorState = (PowerGridSimulatorState)parameter;

            viewModel.ChangeSimulatorState(simulatorState.StartStopSimulator());
        }
    }
}
