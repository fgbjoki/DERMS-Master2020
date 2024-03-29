﻿using ClientUI.ViewModels;
using ClientUI.ViewModels.DEREnergyBalance;
using ClientUI.ViewModels.DEROptimalCommanding;
using ClientUI.ViewModels.Forecast.Production;
using ClientUI.ViewModels.Schema;
using ClientUI.ViewModels.Summaries.DERGroupSummary;
using ClientUI.ViewModels.Summaries.NetworkSummary;
using ClientUI.ViewModels.Summaries.NetworkSummary.Cache;
using ClientUI.ViewModels.Summaries.RemotePointSummaries;
using System.Collections.Generic;

namespace ClientUI.SummaryCreator
{
    public enum ContentType
    {
        NoActionContent,
        AnalogRemotePointSummary,
        AnalogRemotePointCommandingWindow,
        DiscreteRemotePointSummary,
        DiscreteRemotePointCommandingWindow,
        BrowseSchema,
        DERGroupSummary,
        NetworkModelSummary,
        ProductionForecast,
        DEROptimalCommanding,
        DEREnergyBalanceCommanding
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
            CreateAdditionalViewModels(viewModels);

            return viewModels;
        }

        private void CreateAdditionalViewModels(List<ContentViewModel> viewModels)
        {     
            viewModels.Add(new BrowseSchemaViewModel());
            viewModels.Add(new ProductionForecastViewModel());
            viewModels.Add(new DEROptimalCommandingViewModel());
            viewModels.Add(new DEREnergyBalanceForecastViewModel());
        }

        private void CreateSummaryViewModels(List<ContentViewModel> viewModels)
        {
            viewModels.Add(new AnalogRemotePointSummaryViewModel());
            viewModels.Add(new DiscreteRemotePointSummaryViewModel());
            viewModels.Add(new DERGroupSummaryViewModel());
            viewModels.Add(new NetworkModelSummaryViewModel(new ViewModelCache()));
        }
    }
}
