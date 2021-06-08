using Common.UIDataTransferObject.DEROptimalCommanding;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientUI.ViewModels.DEROptimalCommanding.OptimalCommanding.CommandSummaryViewModels
{
    public abstract class BaseCommandSummaryViewModel : BaseViewModel
    {
        private bool commandingSequenceValid;

        public BaseCommandSummaryViewModel()
        {
            SuggestedValues = new ObservableCollection<BaseSummarySuggestedValueViewModel>();
        }

        public ObservableCollection<BaseSummarySuggestedValueViewModel> SuggestedValues { get; set; }

        public void PopulateSuggestedCommandList(SuggsetedCommandSequenceDTO commandSequence)
        {
            SuggestedValues.Clear();
            ShowSuggestedCommands(commandSequence);
            CommandingSequenceValid = commandSequence.CommandingSequenceValid;
        }

        protected abstract BaseSummarySuggestedValueViewModel CreateSuggestedSample(SuggestedCommand suggestedCommand);

        private void ShowSuggestedCommands(SuggsetedCommandSequenceDTO commandSequence)
        {
            foreach (var suggestedCommand in commandSequence.SuggestedCommands)
            {
                BaseSummarySuggestedValueViewModel suggestedCommandItem = CreateSuggestedSample(suggestedCommand);
                SuggestedValues.Add(suggestedCommandItem);
            }
        }


        public bool CommandingSequenceValid
        {
            get { return commandingSequenceValid; }
            set
            {
                if (commandingSequenceValid != value)
                {
                    SetProperty(ref commandingSequenceValid, value);
                }
            }
        }

        public CommandSequenceRequest CreateCommandSequence()
        {
            CommandSequenceRequest commandSequenceRequest = new CommandSequenceRequest();

            foreach (var suggestedCommand in SuggestedValues)
            {
                commandSequenceRequest.CommandRequestSequence.Add(suggestedCommand.CreateCommandRequest());
            }

            return commandSequenceRequest;
        }
    }
}
