using ClientUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientUI.SummaryCreator
{
    public enum SummaryType
    {
        RemotePointSummary
    }

    public class SummaryViewModelContainer
    {
        private Dictionary<SummaryType, ContentViewModel> viewModelsCreated;

        public SummaryViewModelContainer()
        {
            viewModelsCreated = new Dictionary<SummaryType, ContentViewModel>();
        }

        public ContentViewModel GetContent(SummaryType summaryType)
        {
            ContentViewModel contentViewModel;
            if (!viewModelsCreated.TryGetValue(summaryType, out contentViewModel))
            {
                contentViewModel = CreateViewModel(summaryType);
            }

            return contentViewModel;
        }

        private ContentViewModel CreateViewModel(SummaryType summaryType)
        {
            switch (summaryType)
            {
                case SummaryType.RemotePointSummary:
                    return new RemotePointSummaryViewModel();
                default:
                    return null;
            }
        }
    }
}
