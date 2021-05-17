using ClientUI.Common;
using ClientUI.Events.ChangeSummary;
using ClientUI.Events.ContentMenu;
using ClientUI.SummaryCreator;
using ClientUI.ViewModels;
using MaterialDesignThemes.Wpf;
using System.Collections.ObjectModel;
using System.Linq;
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
        private ContentItem selectedItem;

        public ContentMenuViewModel()
        {
            SummaryManager.Instance.EventAggregator.GetEvent<OpenSummaryEvent>().Subscribe(OpenSummary);

            ContentItem scadaContent = new ContentItem(new SummaryWrapper("SCADA", ContentType.SCADA, PackIconKind.AlarmCheck));
            scadaContent.AddChild(new ContentItem(new SummaryWrapper("Analog Remote Point", ContentType.AnalogRemotePointSummary, PackIconKind.SettingsInputComponent)));
            scadaContent.AddChild(new ContentItem(new SummaryWrapper("Discrete Remote Point", ContentType.DiscreteRemotePointSummary, PackIconKind.Switch)));

            Summaries = new ObservableCollection<ContentItem>()
            {
                scadaContent,
                new ContentItem(new SummaryWrapper("Schema", ContentType.BrowseSchema, PackIconKind.ViewAgenda)),
                new ContentItem(new SummaryWrapper("DERGroup", ContentType.DERGroupSummary, PackIconKind.Group))
            };
        }

        public ContentItem SelectedItem
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
        public ObservableCollection<ContentItem> Summaries { get; set; }

        public void OpenSummaryCommand()
        {
            if (selectedItem.SummaryInfo.SummaryType == ContentType.SCADA)
            {
                return;
            }

            SummaryManager.Instance.EventAggregator.GetEvent<ChangeSummaryEvent>().Publish(new ChangeSummaryEventArgs(selectedItem.SummaryInfo.SummaryType));
        }

        private void OpenSummary(OpenSummaryEvetnArgs summaryTypeArgs)
        {
            if (selectedItem.SummaryInfo.SummaryType != summaryTypeArgs.ContentType)
            {
                SelectedItem = Summaries.First(x => x.SummaryInfo.SummaryType == summaryTypeArgs.ContentType);
            }
        }
    }
}
