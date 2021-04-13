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
    public class StartSimulationCommand : ICommand
    {
        private IPowerGridSimulatorViewModel viewModel;

        public StartSimulationCommand(IPowerGridSimulatorViewModel viewModel)
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


            return simulatorState.CanStartSimulation();
        }

        public void Execute(object parameter)
        {
            PowerGridSimulatorState simulatorState = (PowerGridSimulatorState)parameter;
            // TODO

            viewModel.ChangeSimulatorState(simulatorState.StartSimulation());
        }
    }
}
