using ClientUI.Common;
using ClientUI.Events.OpenCommandingWindow;
using System.Windows.Input;
using System;
using System.Timers;
using Common.Communication;
using Common.ServiceInterfaces.UIAdapter.SummaryJobs;
using Common.UIDataTransferObject.RemotePoints;
using System.Collections.Generic;

namespace ClientUI.ViewModels.CommandingWindow
{
    public class AnalogRemotePointCommandingViewModel : EntityCommandingViewModel
    {
        private float value;

        private WCFClient<IAnalogRemotePointSummaryJob> summaryJob;

        public AnalogRemotePointCommandingViewModel(AnalogRemotePointOpenCommandingWindowEventArgs analogRemotePoint) : base("Analog Remote Point Commanding Window")
        {
            GlobalId = analogRemotePoint.GlobalId;
            Name = analogRemotePoint.Name;
            Address = analogRemotePoint.Address;

            SendCommandCommand = new RelayCommand(ExecuteCommand, CanExecuteSendCommand);

            summaryJob = new WCFClient<IAnalogRemotePointSummaryJob>("uiAdapterEndpointAnalog");
            FetchContent();
        }

        public long GlobalId { get; set; }

        public string Name { get; set; }

        public int Address { get; set; }

        public float Value
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

        protected override void FetchContent()
        {
            AnalogRemotePointSummaryDTO item = null;
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
