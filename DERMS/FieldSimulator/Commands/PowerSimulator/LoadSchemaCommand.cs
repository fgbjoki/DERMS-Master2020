using FieldSimulator.Commands.PowerSimulator.State;
using FieldSimulator.ViewModel;
using System;
using System.Windows.Input;

namespace FieldSimulator.Commands.PowerSimulator
{
    public class LoadSchemaCommand : ICommand
    {
        private IPowerGridSimulatorViewModel viewModel;

        public LoadSchemaCommand(IPowerGridSimulatorViewModel viewModel)
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


            return simulatorState.CanLoadSchema();
        }

        public void Execute(object parameter)
        {
            PowerGridSimulatorState simulatorState = (PowerGridSimulatorState)parameter;

            viewModel.ChangeSimulatorState(simulatorState.LoadSchema());
        }
    }
}
