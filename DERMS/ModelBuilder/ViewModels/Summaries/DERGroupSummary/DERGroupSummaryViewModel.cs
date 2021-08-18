using ClientUI.Events.OpenCommandingWindow;
using ClientUI.Models.DERGroup;
using Common.UIDataTransferObject.DERGroup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientUI.SummaryCreator;
using Common.Communication;
using Common.ServiceInterfaces.UIAdapter.SummaryJobs;
using System.Windows.Input;
using ClientUI.Common;
using System.Collections.ObjectModel;
using ClientUI.Common.ViewType;
using ClientUI.Common.DERGroup;
using Common.AbstractModel;
using ClientUI.CustomControls;

namespace ClientUI.ViewModels.Summaries.DERGroupSummary
{
    public class DERGroupSummaryViewModel : CommandingSummaryViewModel<DERGroupSummaryItem, DERGroupOpenCommandingWindowEvent, DERGroupOpenCommandingWindowEventArgs>
    {
        private WCFClient<IDERGroupSummaryJob> summaryJob;
        private ViewTypeOption selectedViewType;

        private TileFillOption selectedTileFill;

        private TechnologyType selectedTechnology;

        public DERGroupSummaryViewModel() : base("DERGroup Summary", ContentType.DERGroupSummary)
        {
            summaryJob = new WCFClient<IDERGroupSummaryJob>("uiAdapterDERGroupEndpoint");

            FilteredItems = new GIDMappedObservableCollection<DERGroupSummaryItem>();

            DataGridItemDoubleClicked = new RelayCommand(DataGridItemSelected, null);

            InitializeViewTypeOptions();
            InitializeTileFillOptions();

            InitializeTechnologyFilter();
        }

        private void InitializeTechnologyFilter()
        {
            TechnologyTypes = new ObservableCollection<TechnologyType>()
            {
                new TechnologyType() { Name = "All", DMSType = DMSType.GEOGRAPHICALREGION },
                new TechnologyType() { Name = "Solar", DMSType = DMSType.SOLARGENERATOR },
                new TechnologyType() { Name = "Wind", DMSType = DMSType.WINDGENERATOR },
            };

            SelectedTechnologyType = TechnologyTypes[0];
        }

        public ICommand DataGridItemDoubleClicked { get; set; }

        public ObservableCollection<ViewTypeOption> ViewTypeOptions { get; set; }

        public ObservableCollection<TileFillOption> TileFillOptions { get; set; }

        public ObservableCollection<TechnologyType> TechnologyTypes { get; set; }

        public GIDMappedObservableCollection<DERGroupSummaryItem> FilteredItems { get; set; }

        public ViewTypeOption SelectedViewTypeOption
        {
            get { return selectedViewType; }
            set
            {
                if (selectedViewType != value)
                {
                    SetProperty(ref selectedViewType, value);
                }
            }
        }

        public TileFillOption SelectedTileFillOption
        {
            get { return selectedTileFill; }
            set
            {
                if (selectedTileFill != value)
                {
                    SetProperty(ref selectedTileFill, value);
                }
            }
        }

        public TechnologyType SelectedTechnologyType
        {
            get { return selectedTechnology; }
            set
            {
                if (selectedTechnology != value)
                {
                    SetProperty(ref selectedTechnology, value);
                    FilterItems();
                }
            }
        }

        protected override List<DERGroupSummaryItem> GetEntitiesFromService()
        {
            List<DERGroupSummaryDTO> items;
            try
            {
                items = summaryJob.Proxy.GetAllAnalogEntities();
            }
            catch
            {
                items = new List<DERGroupSummaryDTO>();
            }

            List<DERGroupSummaryItem> summaryItems = new List<DERGroupSummaryItem>(items.Count);

            foreach (var item in items)
            {
                DERGroupSummaryItem summaryItem = new DERGroupSummaryItem();
                summaryItem.Update(item);

                summaryItems.Add(summaryItem);
            }

            return summaryItems;
        }

        protected override DERGroupOpenCommandingWindowEventArgs TransformGridDataToEventArg(DERGroupSummaryItem selectedItem)
        {
            return new DERGroupOpenCommandingWindowEventArgs()
            {
                GlobalId = selectedItem.GlobalId
            };
        }

        private void InitializeViewTypeOptions()
        {
            ViewTypeOptions = new ObservableCollection<ViewTypeOption>()
            {
                new ViewTypeOption("Data grid", ViewTypeEnum.Grid),
                new ViewTypeOption("Tile", ViewTypeEnum.Tile)
            };
            SelectedViewTypeOption = ViewTypeOptions[0];
        }

        private void InitializeTileFillOptions()
        {
            TileFillOptions = new ObservableCollection<TileFillOption>()
            {
                new TileFillOption("Active power load", TileFillEnum.ActivePower),
                new TileFillOption("State of charge", TileFillEnum.EnergyStateOfCharge)
            };

            SelectedTileFillOption = TileFillOptions[0];
        }

        private void DataGridItemSelected(object item)
        {
            if (SelectedItem == null)
            {
                return;
            }

            RaiseEventForCommandingWindow(SelectedItem);
        }

        private void FilterItems()
        {
            FilteredItems.Clear();

            if (selectedTechnology.Name == "All")
            {
                ClearFilter();
                return;
            }

            foreach (var item in Items)
            {
                if (item.Generator.DMSType == selectedTechnology.DMSType)
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

        protected override void UpdateEntities(List<DERGroupSummaryItem> entities)
        {
            base.UpdateEntities(entities);

            foreach (var entity in entities)
            {
                if (selectedTechnology.Name != "All" && selectedTechnology.DMSType != entity.Generator.DMSType)
                {
                    continue;
                }

                FilteredItems.AddOrUpdateEntity(entity);
            }
        }
    }
}
