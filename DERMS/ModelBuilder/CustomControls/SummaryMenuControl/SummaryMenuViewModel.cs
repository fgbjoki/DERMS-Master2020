using ClientUI.Common;
using ClientUI.SummaryCreator;
using ClientUI.ViewModels;
using MaterialDesignThemes.Wpf;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ClientUI.CustomControls
{
    public class SummaryWrapper
    {
        public SummaryWrapper(string summaryName, SummaryType summaryType, PackIconKind iconKind)
        {
            SummaryType = summaryType;
            SummaryName = summaryName;
            IconKind = iconKind;
        }

        public SummaryType SummaryType { get; private set; }
        public string SummaryName { get; private set; }
        public PackIconKind IconKind { get; set; }
    }

    public class SummaryMenuViewModel : BaseViewModel
    {
        private SummaryWrapper selectedItem;

        public SummaryMenuViewModel()
        {
            Summaries = new ObservableCollection<SummaryWrapper>()
            {
                new SummaryWrapper("Remote Point", SummaryType.RemotePointSummary, PackIconKind.Arrow)
            };

        }

        public SummaryWrapper SelectedItem
        {
            get { return selectedItem; }
            set
            {
                if (selectedItem != value)
                {
                    selectedItem = value;
                    OpenSummaryCommand();
                }
            }
        }
        public ObservableCollection<SummaryWrapper> Summaries { get; set; }

        public void OpenSummaryCommand()
        {
            SummaryManager.Instance.EventAggregator.GetEvent<ChangeSummaryEvent>().Publish(new ChangeSummaryEventArgs(selectedItem.SummaryType));
        }
    }
}
