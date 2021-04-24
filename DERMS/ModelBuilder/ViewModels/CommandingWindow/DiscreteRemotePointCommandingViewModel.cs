using ClientUI.Common;
using ClientUI.Events.OpenCommandingWindow;
using System.Windows.Input;
using System;
using Common.Communication;
using Common.ServiceInterfaces.UIAdapter.SummaryJobs;
using Common.UIDataTransferObject.RemotePoints;

namespace ClientUI.ViewModels.CommandingWindow
{
    public class DiscreteRemotePointCommandingViewModel : EntityCommandingViewModel
    {
        private int value;
        private WCFClient<IDiscreteRemotePointSummaryJob> summaryJob;

        public DiscreteRemotePointCommandingViewModel(DiscreteRemotePointOpenCommandingWindowEventArgs discreteRemotePoint) : base("Discrete Remote Point Commanding Window")
        {
            GlobalId = discreteRemotePoint.GlobalId;
            Name = discreteRemotePoint.Name;
            Address = discreteRemotePoint.Address;
            NormalValue = discreteRemotePoint.NormalValue;

            SendCommandCommand = new RelayCommand(ExecuteCommand, CanExecuteSendCommand);

            summaryJob = new WCFClient<IDiscreteRemotePointSummaryJob>("uiAdapterEndpointDiscrete");
            RefreshContent();
        }

        public long GlobalId { get; set; }

        public string Name { get; set; }

        public int Address { get; set; }

        public int Value
        {
            get { return value; }
            set
            {
                if (this.value == value)
                {
                    return;
                }

                SetProperty(ref this.value, value);
            }
        }

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

            int result;
            if (stringParam == null || !int.TryParse(stringParam, out result))
            {
                return false;
            }

            return true;
        }

        protected override void RefreshContent()
        {
            DiscreteRemotePointSummaryDTO item = null;
            try
            {
                item = summaryJob.Proxy.GetEntity(GlobalId);
            }
            catch { }

            if (item == null)
            {
                return;
            }

            Value = item.Value;
        }
    }
}
