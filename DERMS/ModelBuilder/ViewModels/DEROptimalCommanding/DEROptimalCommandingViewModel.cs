using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientUI.SummaryCreator;
using System.Collections.ObjectModel;
using ClientUI.ViewModels.DEROptimalCommanding.OptimalCommanding;
using ClientUI.ViewModels.DEROptimalCommanding.OptimalCommanding.CommandParameterViewModels;
using ClientUI.ViewModels.DEROptimalCommanding.OptimalCommanding.CommandSummaryViewModels;
using System.Windows.Input;
using ClientUI.Common;
using Common.Communication;
using Common.ServiceInterfaces.UIAdapter;
using System.Windows;
using Common.UIDataTransferObject.DEROptimalCommanding;

namespace ClientUI.ViewModels.DEROptimalCommanding
{
    public class DEROptimalCommandingViewModel : ContentViewModel
    {
        private WCFClient<IDEROptimalCommandingProxy> derOptimalCommanding;
        private CommandingTypeOption commandingTypeOption;
        private BaseCommandParameterViewModel commandingParameterViewModel;
        private BaseCommandSummaryViewModel suggestedValuesSummaryViewModel;

        public DEROptimalCommandingViewModel() : base("DER optimal commanding", ContentType.DEROptimalCommanding)
        {
            derOptimalCommanding = new WCFClient<IDEROptimalCommandingProxy>("uiDEROptimalCommanding");
            InitializeCommandingTypes();

            CalculateCommand = new RelayCommand(ExecuteCalculate, CanCalculateExecute);
            ClearSuggestedCommands = new RelayCommand(ExecuteClearSuggestedValues, CanClearSuggestedValuesExecute);
            SendCommandSequenceCommand = new RelayCommand(ExecuteSendCommandSequence, CanSendCommandSequenceExecute);
        }

        private void InitializeCommandingTypes()
        {
            CommandingTypes = new ObservableCollection<CommandingTypeOption>()
            {
                new CommandingTypeOption("Nominal power percentage", OptimalCommandingType.NominalPower),
                new CommandingTypeOption("Active power reserve", OptimalCommandingType.Reserve)
            };

            SelectedCommandingType = CommandingTypes[0];
        }

        public ICommand CalculateCommand { get; set; }

        public ICommand ClearSuggestedCommands { get; set; }

        public ICommand SendCommandSequenceCommand { get; set; }

        public ObservableCollection<CommandingTypeOption> CommandingTypes { get; set; }

        public CommandingTypeOption SelectedCommandingType
        {
            get { return commandingTypeOption; }
            set
            {
                if (commandingTypeOption != value)
                {
                    SetProperty(ref commandingTypeOption, value);
                    ChangeCommandingParameterViewModel();
                    ChangeCommandSummaryViewModel();
                }
            }
        }

        public BaseCommandParameterViewModel CommandingParameterSettingViewModel
        {
            get { return commandingParameterViewModel; }
            set
            {
                if (commandingParameterViewModel != value)
                {
                    SetProperty(ref commandingParameterViewModel, value);
                }
            }
        }

        public BaseCommandSummaryViewModel SuggestedValuesSummaryViewModel
        {
            get { return suggestedValuesSummaryViewModel; }
            set
            {
                if (suggestedValuesSummaryViewModel != value)
                {
                    SetProperty(ref suggestedValuesSummaryViewModel, value);
                }
            }
        }

        private void ChangeCommandingParameterViewModel()
        {
            if (SelectedCommandingType == null)
            {
                return;
            }

            switch (SelectedCommandingType.OptimalCommandingType)
            {
                case OptimalCommandingType.NominalPower:
                    CommandingParameterSettingViewModel = new NominalPowerBasedOptimalCommandingViewModel();
                    break;
                case OptimalCommandingType.Reserve:
                    CommandingParameterSettingViewModel = new ReserveBasedOptimalCommandingViewModel();
                    break;
                default:
                    break;
            }
        }

        private void ChangeCommandSummaryViewModel()
        {
            if (SelectedCommandingType == null)
            {
                return;
            }

            switch (SelectedCommandingType.OptimalCommandingType)
            {
                case OptimalCommandingType.NominalPower:
                    SuggestedValuesSummaryViewModel = new NominalPowerBasedCommandSummaryViewModel();
                    break;
                case OptimalCommandingType.Reserve:
                    SuggestedValuesSummaryViewModel = new ReserveBasedCommandSummaryViewModel();
                    break;
                default:
                    break;
            }
        }

        private bool CanCalculateExecute(object parameter)
        {
            try
            {
                object[] parameters = parameter as object[];
                if (parameters == null)
                {
                    return false;
                }

                bool setpointValid = (bool)parameters[0];
                float setPointValue = (float)parameters[1];

                return setpointValid && setPointValue != 0;
            }
            catch
            {
                return false;
            }
        }

        private void ExecuteCalculate(object parameter)
        {
            try
            {
                object[] parameters = parameter as object[];
                float setPointValue = (float)parameters[1];
                Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (Action)(() => 
                {
                    var response = derOptimalCommanding.Proxy.GetSuggestedCommandSequence(CommandingParameterSettingViewModel.RequestType, commandingParameterViewModel.SetPoint);
                    SuggestedValuesSummaryViewModel.PopulateSuggestedCommandList(response);
                }));              
            }
            catch
            {
                
            }
        }

        private bool CanClearSuggestedValuesExecute(object parameter)
        {
            return SuggestedValuesSummaryViewModel?.SuggestedValues.Count > 0;
        }

        private void ExecuteClearSuggestedValues(object parameter)
        {
            SuggestedValuesSummaryViewModel.SuggestedValues.Clear();
        }

        private bool CanSendCommandSequenceExecute(object parameter)
        {
            bool sequenceExists = SuggestedValuesSummaryViewModel?.SuggestedValues.Count > 0;
            bool commandingValid = SuggestedValuesSummaryViewModel == null ? false : SuggestedValuesSummaryViewModel.CommandingSequenceValid;

            return sequenceExists && commandingValid;
        }

        private void ExecuteSendCommandSequence(object parameter)
        {
            Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (Action)(() =>
            {
                CommandSequenceRequest commandSequence = SuggestedValuesSummaryViewModel.CreateCommandSequence();
                try
                {
                    var feedback = derOptimalCommanding.Proxy.ExecuteCommandSequence(commandSequence);
                    MessageBox.Show(feedback.Message, "Execution information", MessageBoxButton.OK, feedback.CommandExecuted ? MessageBoxImage.Information : MessageBoxImage.Error);
                }
                catch
                {
                    MessageBox.Show("Couldn't sent command sequence, try again later.", "Execution information", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                SuggestedValuesSummaryViewModel.SuggestedValues.Clear();
            }));
        }
    }
}
