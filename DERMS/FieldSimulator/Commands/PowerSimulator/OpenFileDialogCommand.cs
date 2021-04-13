using FieldSimulator.Commands.PowerSimulator.State;
using FieldSimulator.ViewModel;
using System;
using System.Windows.Forms;
using System.Windows.Input;

namespace FieldSimulator.Commands.PowerSimulator
{
    public class OpenFileDialogCommand : ICommand
    {
        private IPowerGridSimulatorViewModel viewModel;

        public OpenFileDialogCommand(IPowerGridSimulatorViewModel viewModel)
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

            return simulatorState.CanLoadFile();
        }

        public void Execute(object parameter)
        {
            PowerGridSimulatorState simulatorState = parameter as PowerGridSimulatorState;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "XML Files|*.xml";
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    viewModel.ChangeSimulatorState(simulatorState.LoadFile(openFileDialog.FileName));
                }
            }
        }
    }
}
