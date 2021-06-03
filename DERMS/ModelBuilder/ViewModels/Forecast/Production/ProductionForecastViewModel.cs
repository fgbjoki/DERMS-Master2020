using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientUI.SummaryCreator;
using LiveCharts;
using System.Threading;
using Common.UIDataTransferObject.Forecast.Production;
using Common.Communication;
using Common.ServiceInterfaces.UIAdapter;
using System.Collections.ObjectModel;
using ClientUI.Common;
using Common.AbstractModel;
using ClientUI.ViewModels.Summaries;
using ClientUI.Models;
using Common.ServiceInterfaces.UIAdapter.SummaryJobs;
using Common.UIDataTransferObject.NetworkModel;
using ClientUI.CustomControls;
using ClientUI.ViewModels.Forecast.Production.ProductionSeriesManager;

namespace ClientUI.ViewModels.Forecast.Production
{
    public class ProductionForecastViewModel : SummaryViewModel<IdentifiedObject>
    {
        private List<DMSType> entitiesOfIntereset;
        private WCFClient<IProductionForecast> client;
        private WCFClient<INetworkModelSummaryJob> networkModel;

        private ProductionForecastDTO fetchedData;

        private DTOConverter dtoConverter;

        private ProductionSeriesManager.ProductionSeriesManager seriesManager;

        private ProductionForecastOption selectedProductionType;
        private EntityFilterOption selectedEntityFilterOption;
        private IdentifiedObject selectedEntity;

        public ProductionForecastViewModel() : base("Production Forecast", ContentType.ProductionForecast)
        {
            entitiesOfIntereset = new List<DMSType>() { DMSType.SOLARGENERATOR, DMSType.WINDGENERATOR };

            FilteredEntityItems = new GIDMappedObservableCollection<IdentifiedObject>();

            TotalProduction = new ChartValues<float>();
            SolarProduction = new ChartValues<float>();
            WindProduction = new ChartValues<float>();
            EntityProduction = new ChartValues<float>();

            DisplayControl = new DisplayTypeControl();
            seriesManager = new ProductionSeriesManager.ProductionSeriesManager(TotalProduction, SolarProduction, WindProduction, EntityProduction, DisplayControl);
            dtoConverter = new DTOConverter();

            client = new WCFClient<IProductionForecast>("uiAdapterProductionForecast");
            networkModel = new WCFClient<INetworkModelSummaryJob>("uiNetworkModelEndpoint");

            InitializeProductionTypeFilters();
            InitializeFilterOptions();
        }

        public ProductionForecastOption SelectedProductionType
        {
            get { return selectedProductionType; }
            set
            {
                if (selectedProductionType?.ProductionForecastType != value.ProductionForecastType)
                {
                    SetProperty(ref selectedProductionType, value);
                    if (selectedProductionType.ProductionForecastType == ProductionForecastType.Entity)
                    {
                        DisplayControl.HideAll();
                    }
                    else if (selectedProductionType.ProductionForecastType == ProductionForecastType.Total)
                    {
                        DisplayControl.DisplayAll();
                        DisplayControl.DisplayEntity = false;
                    }
                }
            }
        }

        public EntityFilterOption SelectedEntityFilterOption
        {
            get { return selectedEntityFilterOption; }
            set
            {
                if (selectedEntityFilterOption?.DMSType != value.DMSType)
                {
                    SetProperty(ref selectedEntityFilterOption, value);
                    FilterItems();
                }
            }
        }

        public IdentifiedObject SelectedEntity
        {
            get { return selectedEntity; }
            set
            {
                if (selectedEntity != value)
                {
                    SetProperty(ref selectedEntity, value);
                    PlotGraphForEntity(selectedEntity);
                }
            }
        }

        public DisplayTypeControl DisplayControl { get; set; }

        public ObservableCollection<ProductionForecastOption> ProductionTypes { get; set; }

        public ObservableCollection<EntityFilterOption> EntityFilterOptions { get; set; }

        public GIDMappedObservableCollection<IdentifiedObject> FilteredEntityItems { get; set; }

        public ChartValues<float> TotalProduction { get; set; }

        public ChartValues<float> SolarProduction { get; set; }

        public ChartValues<float> WindProduction { get; set; }

        public ChartValues<float> EntityProduction { get; set; }

        public override void StartProcessing()
        {
            base.StartProcessing();

            FetchDataFromService();
            if (fetchedData == null)
            {
                return;
            }

            var convertedData = dtoConverter.Convert(fetchedData);

            foreach (var data in convertedData)
            {
                seriesManager.PopulateChart(data.Item1, data.Item2);
            }
        }

        protected override List<IdentifiedObject> GetEntitiesFromService()
        {
            List<NetworkModelEntityDTO> items;
            try
            {
                items = networkModel.Proxy.GetAllEntities(entitiesOfIntereset);
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
                if (!String.IsNullOrEmpty(selectedEntityFilterOption.Name) && selectedEntityFilterOption.DMSType != entity.DMSType)
                {
                    continue;
                }

                FilteredEntityItems.AddOrUpdateEntity(entity);
            }
        }

        private void FetchDataFromService()
        {
            try
            {
                fetchedData = client.Proxy.GetProductionForecast(24);
            }
            catch { }
        }

        private void InitializeProductionTypeFilters()
        {
            ProductionTypes = new ObservableCollection<ProductionForecastOption>()
            {
                new ProductionForecastOption("Total", ProductionForecastType.Total),
                new ProductionForecastOption("Entity", ProductionForecastType.Entity)
            };

            SelectedProductionType = ProductionTypes[0];
        }

        private void InitializeFilterOptions()
        {
            EntityFilterOptions = new ObservableCollection<EntityFilterOption>()
            {
                new EntityFilterOption("", DMSType.MASK_TYPE),
                new EntityFilterOption("Solar panel", DMSType.SOLARGENERATOR),
                new EntityFilterOption("Wind generator", DMSType.WINDGENERATOR)
            };

            SelectedEntityFilterOption = EntityFilterOptions[0];
        }
  
        private void PlotGraphForEntity(IdentifiedObject selectedEntity)
        {
            if (selectedEntity == null)
            {
                DisplayControl.HideAll();
                return;
            }

            DisplayControl.Display(PowerType.Entity);
            seriesManager.PopulateChart(PowerType.Entity, dtoConverter.CreateValues(fetchedData.GeneratorProductionForecasts[selectedEntity.GlobalId]));
        }

        private void FilterItems()
        {
            FilteredEntityItems.Clear();

            if (String.IsNullOrEmpty(SelectedEntityFilterOption.Name))
            {
                ClearFilter();
                return;
            }

            foreach (var item in Items)
            {
                if (item.DMSType == SelectedEntityFilterOption.DMSType)
                {
                    FilteredEntityItems.AddOrUpdateEntity(item);
                }
            }
        }

        private void ClearFilter()
        {
            foreach (var item in Items)
            {
                FilteredEntityItems.AddOrUpdateEntity(item);
            }
        }
    }
}
