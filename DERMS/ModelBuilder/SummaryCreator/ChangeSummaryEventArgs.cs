
namespace ClientUI.SummaryCreator
{
    public class ChangeSummaryEventArgs
    {
        public ChangeSummaryEventArgs(SummaryType summaryType)
        {
            SummaryType = summaryType;
        }

        public SummaryType SummaryType { get; set; }
    }
}
