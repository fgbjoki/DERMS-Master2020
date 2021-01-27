using ClientUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientUI.SummaryCreator
{
    public enum ContentType
    {
        RemotePointSummary
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
            return new List<ContentViewModel>()
            {
                new RemotePointSummaryViewModel()
            };
        }
    }
}
