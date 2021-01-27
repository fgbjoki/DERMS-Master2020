
namespace ClientUI.SummaryCreator
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
