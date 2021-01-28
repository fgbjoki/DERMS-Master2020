
using ClientUI.SummaryCreator;

namespace ClientUI.Events.ChangeSummary
{
    public class ChangeSummaryEventArgs
    {
        public ChangeSummaryEventArgs(ContentType summaryType)
        {
            SummaryType = summaryType;
        }

        public ContentType SummaryType { get; set; }
    }
}
