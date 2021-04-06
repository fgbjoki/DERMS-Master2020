using ClientUI.Common;
using ClientUI.Events.OpenCommandingWindow;
using System.Windows.Input;

namespace ClientUI.ViewModels.CommandingWindow
{
    public class DiscreteRemotePointCommandingViewModel : CommandingViewModel
    {
        public DiscreteRemotePointCommandingViewModel(DiscreteRemotePointOpenCommandingWindowEventArgs discreteRemotePoint) : base("Discrete Remote Point Commanding Window")
        {
            GlobalId = discreteRemotePoint.GlobalId;
            Name = discreteRemotePoint.Name;
            Address = discreteRemotePoint.Address;
            Value = discreteRemotePoint.Value;
            NormalValue = discreteRemotePoint.NormalValue;

            SendCommandCommand = new RelayCommand(ExecuteCommand, CanExecuteSendCommand);
        }

        public long GlobalId { get; set; }

        public string Name { get; set; }

        public int Address { get; set; }

        public int Value { get; set; }

        // not sure
        public int NormalValue { get; set; }

        public int NewCommandingValue { get; set; }

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
