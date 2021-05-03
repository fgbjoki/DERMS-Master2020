using ClientUI.Common;
using ClientUI.Events.OpenCommandingWindow;
using System.Windows.Input;
using System;
using Common.Communication;
using Common.ServiceInterfaces.UIAdapter.SummaryJobs;
using Common.UIDataTransferObject.RemotePoints;
using ClientUI.Commanding;
using System.Timers;
using System.Windows;
using Common.DataTransferObjects;

namespace ClientUI.ViewModels.CommandingWindow
{
    public class DiscreteRemotePointCommandingViewModel : EntityCommandingViewModel
    {
        private int value;
        private int newCommandingValue;
        private bool isCommandValid = false;
        private WCFClient<IDiscreteRemotePointSummaryJob> summaryJob;

        private Timer feedBackMessageTimer;

        public DiscreteRemotePointCommandingViewModel(DiscreteRemotePointOpenCommandingWindowEventArgs discreteRemotePoint) : base("Discrete Remote Point Commanding Window")
        {
            feedBackMessageTimer = new Timer();
            feedBackMessageTimer.Interval = 1000 * 3;
            feedBackMessageTimer.AutoReset = false;
            feedBackMessageTimer.Elapsed += FeedBackMessageTimer_Elapsed;

            GlobalId = discreteRemotePoint.GlobalId;
            Name = discreteRemotePoint.Name;
            Address = discreteRemotePoint.Address;
            NormalValue = discreteRemotePoint.NormalValue;

            CommandFeedback = new CommandFeedback("", true);
            SendCommandCommand = new RelayCommand(ExecuteSendCommandCommand, CanExecuteSendCommandCommand);
            ValidationCommand = new RelayCommand(ExecuteValidationCommand, CanExecuteValidationCommand);

            summaryJob = new WCFClient<IDiscreteRemotePointSummaryJob>("uiAdapterEndpointDiscrete");
            RefreshContent();
        }
        public CommandFeedback CommandFeedback { get; private set; }

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

        public bool IsCommandValid
        {
            get { return isCommandValid; }
            private set
            {
                if (isCommandValid != value)
                {
                    SetProperty(ref isCommandValid, value);
                }
            }
        }

        public int NormalValue { get; set; }

        public int NewCommandingValue
        {
            get { return newCommandingValue; }
            set
            {
                if (newCommandingValue != value)
                {
                    newCommandingValue = value;
                    IsCommandValid = false;
                }
            }
        }

        public ICommand SendCommandCommand { get; set; }

        public ICommand ValidationCommand { get; set; }


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

        private void ExecuteSendCommandCommand(object parameter)
        {
            CommandFeedbackMessageDTO feedBack = CommandingProxy.Instance.SendBreakerCommand(GlobalId, NewCommandingValue);
            ProcessFeedback(feedBack);
        }

        private bool CanExecuteSendCommandCommand(object parameter)
        {
            string stringParam = parameter as string;

            int result;
            if (stringParam == null || !int.TryParse(stringParam, out result))
            {
                return false;
            }

            bool isValid = result == 0 || result == 1;

            if (!isValid)
            {
                IsCommandValid = false;
            }

            return isValid;
        }

        private void ExecuteValidationCommand(object parameter)
        {
            CommandFeedbackMessageDTO feedBack = CommandingProxy.Instance.ValidateCommand(GlobalId, NewCommandingValue);
            ProcessFeedback(feedBack);
        }

        private void ProcessFeedback(CommandFeedbackMessageDTO feedBack)
        {
            feedBackMessageTimer.Stop();

            CommandFeedback.CommandExecuted = IsCommandValid = feedBack.CommandExecuted;
            CommandFeedback.Message = feedBack.Message;
            CommandFeedback.Visibility = Visibility.Visible;

            feedBackMessageTimer.Start();
        }

        private bool CanExecuteValidationCommand(object parameter)
        {
            string stringParam = parameter as string;

            int result;
            if (stringParam == null || !int.TryParse(stringParam, out result))
            {
                return false;
            }

            bool isValid = result == 0 || result == 1;

            isValid &= Value != result;
            isValid &= Value != NewCommandingValue;

            if (!isValid)
            {
                IsCommandValid = false;
            }

            return isValid;
        }

        private void FeedBackMessageTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            CommandFeedback.Visibility = Visibility.Hidden;
        }
    }
}
