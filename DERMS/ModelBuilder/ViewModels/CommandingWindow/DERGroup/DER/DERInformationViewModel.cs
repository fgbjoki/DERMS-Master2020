using ClientUI.Commanding;
using ClientUI.Common;
using ClientUI.Events.Schema;
using ClientUI.SummaryCreator;
using Common.Communication;
using Common.DataTransferObjects;
using Common.ServiceInterfaces.UIAdapter.SummaryJobs;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;

namespace ClientUI.ViewModels.CommandingWindow.DERGroup.DER
{
    public enum GeneratorType
    {
        Wind,
        Solar
    }

    public abstract class DERInformationViewModel : BaseDERCommandingEntityViewModel
    {
        private string name;
        private string imageUrl;
        private float activePower;
        private float nominalPower;

        private bool isCommandValid = false;

        private float newCommandingValue;

        private Timer feedBackMessageTimer;

        public DERInformationViewModel(long derGlobalId, WCFClient<IDERGroupSummaryJob> derGroupSummary, string imageUrl = "") : base(derGlobalId, derGroupSummary)
        {
            feedBackMessageTimer = new Timer();
            feedBackMessageTimer.Interval = 1000 * 5;
            feedBackMessageTimer.AutoReset = false;
            feedBackMessageTimer.Elapsed += FeedBackMessageTimer_Elapsed;

            this.imageUrl = imageUrl;

            CommandFeedback = new CommandFeedback("", true);

            LocateOnSchemaCommand = new RelayCommand(PublishLocateNodeEvent);
         
            SendCommandCommand = new RelayCommand(ExecuteSendCommandCommand, CanExecuteSendCommandCommand);
            ValidationCommand = new RelayCommand(ExecuteValidationCommand, CanExecuteValidationCommand);
        }

        public string ImageSource
        {
            get { return imageUrl; }
            set
            {
                if (ImageSource != value)
                {
                    SetProperty(ref imageUrl, value);
                }
            }
        }

        public string Name
        {
            get { return name; }
            protected set
            {
                if (name != value)
                {
                    SetProperty(ref name, value);
                }
            }
        }

        public float NominalPower
        {
            get { return nominalPower; }
            set
            {
                if (nominalPower != value)
                {
                    SetProperty(ref nominalPower, value);
                }
            }
        }

        public float ActivePower
        {
            get { return activePower; }
            set
            {
                if (activePower != value)
                {
                    SetProperty(ref activePower, value);
                }
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

        public float NewCommandingValue
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

        public CommandFeedback CommandFeedback { get; private set; }

        public ICommand LocateOnSchemaCommand { get; set; }

        public ICommand SendCommandCommand { get; set; }

        public ICommand ValidationCommand { get; set; }

        protected virtual bool ValidateDifferenceInCommandingValue(float commandingValue)
        {
            return ActivePower != NewCommandingValue;
        }

        private void ExecuteSendCommandCommand(object parameter)
        {
            CommandFeedbackMessageDTO feedBack = CommandingProxy.Instance.DERCommanding.SendCommand(entityGid, NewCommandingValue);
            ProcessFeedback(feedBack);
        }

        private bool CanExecuteSendCommandCommand(object parameter)
        {
            string stringParam = parameter as string;

            float result;
            if (stringParam == null || !float.TryParse(stringParam, out result))
            {
                return false;
            }

            bool isValid = ValidateDifferenceInCommandingValue(result);

            if (!isValid)
            {
                IsCommandValid = false;
            }

            return isValid;
        }

        private void ProcessFeedback(CommandFeedbackMessageDTO feedBack)
        {
            feedBackMessageTimer.Stop();

            CommandFeedback.CommandExecuted = IsCommandValid = feedBack.CommandExecuted;
            CommandFeedback.Message = feedBack.Message;
            CommandFeedback.Visibility = Visibility.Visible;

            feedBackMessageTimer.Start();
        }

        private void ExecuteValidationCommand(object parameter)
        {
            CommandFeedbackMessageDTO feedBack = CommandingProxy.Instance.DERCommanding.ValidateCommand(entityGid, NewCommandingValue);
            ProcessFeedback(feedBack);
        }

        private bool CanExecuteValidationCommand(object parameter)
        {
            string stringParam = parameter as string;

            float result;
            if (stringParam == null || !float.TryParse(stringParam, out result))
            {
                return false;
            }

            bool isValid = ValidateDifferenceInCommandingValue(result);

            if (!isValid)
            {
                IsCommandValid = false;
            }

            return isValid;
        }

        private void PublishLocateNodeEvent(object param)
        {
            SummaryManager.Instance.EventAggregator.GetEvent<SchemaNodeLocateEvent>().Publish(new SchemaNodeLocateEventArgs() { LocatingEntityGid = entityGid });
        }

        private void FeedBackMessageTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            CommandFeedback.Visibility = Visibility.Hidden;
        }
    }
}
