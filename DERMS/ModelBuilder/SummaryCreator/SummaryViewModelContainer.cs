using ClientUI.ViewModels;
using ClientUI.ViewModels.Schema;
using ClientUI.ViewModels.Summaries.DERGroupSummary;
using ClientUI.ViewModels.Summaries.RemotePointSummaries;
using System.Collections.Generic;

namespace ClientUI.SummaryCreator
{
    public enum ContentType
    {
        SCADA,
        AnalogRemotePointSummary,
        AnalogRemotePointCommandingWindow,
        DiscreteRemotePointSummary,
        DiscreteRemotePointCommandingWindow,
        BrowseSchema,
        DERGroupSummary
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
            List<ContentViewModel> viewModels = new List<ContentViewModel>();

            CreateSummaryViewModels(viewModels);
            CreateSchemaViewModels(viewModels);

            return viewModels;
        }

        private void CreateSchemaViewModels(List<ContentViewModel> viewModels)
        {     
            viewModels.Add(new BrowseSchemaViewModel());
        }

        private void CreateSummaryViewModels(List<ContentViewModel> viewModels)
        {
            viewModels.Add(new AnalogRemotePointSummaryViewModel());
            viewModels.Add(new DiscreteRemotePointSummaryViewModel());
            viewModels.Add(new DERGroupSummaryViewModel());
        }
    }
}
