﻿using ClientUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientUI.SummaryCreator;
using ClientUI.CustomControls;
using System.Collections.ObjectModel;
using Common.AbstractModel;
using Common.Communication;
using Common.ServiceInterfaces.UIAdapter.SummaryJobs;
using Common.UIDataTransferObject.NetworkModel;
using ClientUI.ViewModels.Summaries.NetworkSummary.Cache;
using ClientUI.ViewModels.Summaries.NetworkSummary.EntityInformationViewModels;

namespace ClientUI.ViewModels.Summaries.NetworkSummary
{
    public class NetworkModelSummaryViewModel : SummaryViewModel<IdentifiedObject>
    {
        private IViewModelCache viewModelCache;

        private FilterOption selectedFilterOption;

        private WCFClient<INetworkModelSummaryJob> networkModelSummaryJob;

        private BaseNetworkModelEntityInformationViewModel currentViewModel;

        public NetworkModelSummaryViewModel(IViewModelCache viewModelCache) : base("Network Model Summary", ContentType.NetworkModelSummary)
        {
            this.viewModelCache = viewModelCache;

            networkModelSummaryJob = new WCFClient<INetworkModelSummaryJob>("uiNetworkModelEndpoint");

            FilteredItems = new GIDMappedObservableCollection<IdentifiedObject>();

            InitializeFilterOptions();
        }

        private void InitializeFilterOptions()
        {
            FilterOptions = new ObservableCollection<FilterOption>()
            {
                new FilterOption("", DMSType.MASK_TYPE),
                new FilterOption("Wind generator", DMSType.WINDGENERATOR),
                new FilterOption("Solar panel", DMSType.SOLARGENERATOR),
                new FilterOption("Energy storage", DMSType.ENERGYSTORAGE),
                new FilterOption("Consumer", DMSType.ENERGYCONSUMER),
                new FilterOption("Energy source", DMSType.ENERGYSOURCE),
                new FilterOption("Breaker", DMSType.BREAKER),
            };

            SelectedFilterOption = FilterOptions[0];
        }

        public GIDMappedObservableCollection<IdentifiedObject> FilteredItems { get; set; }

        public ObservableCollection<FilterOption> FilterOptions { get; set; }

        public BaseNetworkModelEntityInformationViewModel CurrentViewModel
        {
            get
            {
                return currentViewModel;
            }
            set
            {
                if (currentViewModel != value)
                {
                    SetProperty(ref currentViewModel, value);
                }
            }
        }

        public FilterOption SelectedFilterOption
        {
            get { return selectedFilterOption; }
            set
            {
                if (selectedFilterOption != value)
                {
                    SetProperty(ref selectedFilterOption, value);
                    FilterItems();
                }
            }
        }

        public override IdentifiedObject SelectedItem
        {
            get
            {
                return base.SelectedItem;
            }

            set
            {
                base.SelectedItem = value;
                ChangeViewModel(base.SelectedItem);
            }
        }

        protected override List<IdentifiedObject> GetEntitiesFromService()
        {
            List<NetworkModelEntityDTO> items;
            try
            {
                items = networkModelSummaryJob.Proxy.GetAllEntities();
            }
            catch
            {
                items = new List<NetworkModelEntityDTO>();
            }

            List<IdentifiedObject> summaryItems = new List<IdentifiedObject>(items.Count);

            foreach (var item in items)
            {
                IdentifiedObject summaryItem = new IdentifiedObject();
                summaryItem.Update(item);

                summaryItems.Add(summaryItem);
            }

            return summaryItems;
        }

        protected override void UpdateEntities(List<IdentifiedObject> entities)
        {
            base.UpdateEntities(entities);

            foreach (var entity in entities)
            {
                if (!String.IsNullOrEmpty(selectedFilterOption.Name) && selectedFilterOption.DMSType != entity.DMSType)
                {
                    continue;
                }

                FilteredItems.AddOrUpdateEntity(entity);
            }
        }

        private void ChangeViewModel(IdentifiedObject selectedItem)
        {
            if (selectedItem == null)
            {
                CurrentViewModel = null;
                return;
            }

            var viewModel = viewModelCache.GetViewModel(selectedItem.DMSType);

            try
            {
                NetworkModelEntityDTO dto = networkModelSummaryJob.Proxy.GetEntity(selectedItem.GlobalId);
                viewModel.PopulateFields(dto);
                CurrentViewModel = viewModel;
            }
            catch
            {
                // show error
            }
        }

        private void FilterItems()
        {
            FilteredItems.Clear();

            if (String.IsNullOrEmpty(selectedFilterOption.Name))
            {
                ClearFilter();
                return;
            }
            
            foreach (var item in Items)
            {
                if (item.DMSType == selectedFilterOption.DMSType)
                {
                    FilteredItems.AddOrUpdateEntity(item);
                }
            }
        }

        private void ClearFilter()
        {
            foreach (var item in Items)
            {
                FilteredItems.AddOrUpdateEntity(item);
            }
        }
    }
}
