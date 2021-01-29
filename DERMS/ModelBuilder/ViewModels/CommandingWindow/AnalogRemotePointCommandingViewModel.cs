using ClientUI.Common;
using ClientUI.Events.OpenCommandingWindow;
using System.Windows.Input;

namespace ClientUI.ViewModels.CommandingWindow
{
    public class AnalogRemotePointCommandingViewModel : CommandingViewModel
    {
        public AnalogRemotePointCommandingViewModel(AnalogRemotePointOpenCommandingWindowEventArgs analogRemotePoint) : base("Analog Remote Point Commanding Window")
        {
            GlobalId = analogRemotePoint.GlobalId;
            Name = analogRemotePoint.Name;
            Address = analogRemotePoint.Address;
            Value = analogRemotePoint.Value;

            SendCommandCommand = new RelayCommand(ExecuteCommand, CanExecuteSendCommand);
        }

        public long GlobalId { get; set; }

        public string Name { get; set; }

        public int Address { get; set; }

        public float Value { get; set; }

        public float NewCommandingValue { get; set; }

        public ICommand SendCommandCommand { get; set; }

        private void ExecuteCommand(object parameter)
        {
            // TODO
        }

        private bool CanExecuteSendCommand(object parameter)
        {
            string stringParam = parameter as string;

            double result;
            if (stringParam == null || !double.TryParse(stringParam, out result))
            {
                return false;
            }

            return true;
        }
    }
}
