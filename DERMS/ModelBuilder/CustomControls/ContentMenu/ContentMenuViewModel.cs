using ClientUI.Common;
using ClientUI.Events.ChangeSummary;
using ClientUI.SummaryCreator;
using ClientUI.ViewModels;
using MaterialDesignThemes.Wpf;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ClientUI.CustomControls
{
    public class SummaryWrapper
    {
        public SummaryWrapper(string summaryName, ContentType summaryType, PackIconKind iconKind)
        {
            SummaryType = summaryType;
            SummaryName = summaryName;
            IconKind = iconKind;
        }

        public ContentType SummaryType { get; private set; }
        public string SummaryName { get; private set; }
        public PackIconKind IconKind { get; set; }
    }

    public class ContentMenuViewModel : BaseViewModel
    {
        private SummaryWrapper selectedItem;

        public ContentMenuViewModel()
        {
            Summaries = new ObservableCollection<SummaryWrapper>()
            {
                new SummaryWrapper("Analog Remote Point", ContentType.AnalogRemotePointSummary, PackIconKind.Arrow),
                new SummaryWrapper("Discrete Remote Point", ContentType.DiscreteRemotePointSummary, PackIconKind.Arrow),
                new SummaryWrapper("Schema", ContentType.BrowseSchema, PackIconKind.ClipboardAdd)
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
