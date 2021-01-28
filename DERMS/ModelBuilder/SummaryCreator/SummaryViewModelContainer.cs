using ClientUI.ViewModels;
using ClientUI.ViewModels.Summaries.RemotePointSummaries;
using System.Collections.Generic;

namespace ClientUI.SummaryCreator
{
    public enum ContentType
    {
        AnalogRemotePointSummary,
        AnalogRemotePointCommandingWindow
    }

    public class SummaryViewModelContainer
    {
        private Dictionary<ContentType, ContentViewModel> viewModelsCreated;

        public SummaryViewModelContainer()
        {
            List<ContentViewModel> contentViewModels = CreateViewModels();

            viewModelsCreated = new Dictionary<ContentType, ContentViewModel>(contentViewModels.Count);

            foreach (var contentViewModel in contentViewModels)
            {
                viewModelsCreated.Add(contentViewModel.ContentType, contentViewModel);
            }
        }

        public ContentViewModel GetContent(ContentType summaryType)
        {
            ContentViewModel contentViewModel;
            if (!viewModelsCreated.TryGetValue(summaryType, out contentViewModel))
            {
                return null;
            }

            return contentViewModel;
        }

        private List<ContentViewModel> CreateViewModels()
        {
            return CreateSummaryViewModels();
        }

        private List<ContentViewModel> CreateSummaryViewModels()
        {
            List<ContentViewModel> summaries = new List<ContentViewModel>();
            AnalogRemotePointSummaryViewModel analogSummaryView = new AnalogRemotePointSummaryViewModel();

            summaries.Add(analogSummaryView);


            return summaries;
        }
    }
}
