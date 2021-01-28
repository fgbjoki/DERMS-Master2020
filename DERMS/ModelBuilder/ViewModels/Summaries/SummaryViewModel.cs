using ClientUI.SummaryCreator;
using System.Collections.ObjectModel;

namespace ClientUI.ViewModels.Summaries
{
    public abstract class SummaryViewModel<T> : ContentViewModel
    {
        protected SummaryViewModel(string summaryName, ContentType contentType) : base(summaryName, contentType)
        {
            Items = new ObservableCollection<T>();
        }

        public ObservableCollection<T> Items { get; set; }
    }
}
