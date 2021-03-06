﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientUI.SummaryCreator;
using System.Collections.ObjectModel;
using ClientUI.Models.Schema;
using System.Timers;
using Common.Communication;
using Common.ServiceInterfaces.UIAdapter;
using Common.UIDataTransferObject.Schema;
using ClientUI.CustomControls;
using System.Windows;
using System.Windows.Input;
using ClientUI.Common;

namespace ClientUI.ViewModels.Schema
{
    public class BrowseSchemaViewModel : ContentViewModel, ISchemaNodeLocator
    {
        private Timer timer;

        private SchemaViewModel currentSchemaViewModel;

        private SchemaCreator schemaCreator;

        private WCFClient<ISchema> schemaClient;

        private EnergySourceBrowseSchemaItem selectedEnergySource;

        private SchemaNodeLocator.SchemaNodeLocator schemaNodeLocator;

        public BrowseSchemaViewModel() : base("Schema", ContentType.BrowseSchema)
        {
            timer = new Timer();
            timer.AutoReset = true;
            timer.Elapsed += FetchEnergySources;
            timer.Interval = 1000 * 5; // 10 seconds;

            EnergySources = new GIDMappedObservableCollection<EnergySourceBrowseSchemaItem>();

            schemaClient = new WCFClient<ISchema>("uiAdapterSchemaEndpoint");

            GetSchemaCommand = new RelayCommand(ExecuteGetSchemaCommand, CanGetSchemaCommandExecute);
            ClearSearchingNodeCommand = new RelayCommand(ExecuteClearSearchingNodeCommand, CanClearSearchingNodeCommandExecute);

            schemaCreator = new SchemaCreator();

            schemaNodeLocator = new SchemaNodeLocator.SchemaNodeLocator(this);
        }

        public GIDMappedObservableCollection<EnergySourceBrowseSchemaItem> EnergySources { get; set; }

        public EnergySourceBrowseSchemaItem SelectedEnergySource
        {
            get { return selectedEnergySource; }
            set { SetProperty(ref selectedEnergySource, value); }
        }

        public SchemaViewModel SchemaViewModel
        {
            get { return currentSchemaViewModel; }
            set { SetProperty(ref currentSchemaViewModel, value); }
        }

        public ICommand GetSchemaCommand { get; set; }

        public ICommand ClearSearchingNodeCommand { get; set; }

        public override void StartProcessing()
        {
            FetchEnergySources(this, null);
            timer.Enabled = true;
        }

        public override void StopProcessing()
        {
            timer.Enabled = false;
        }

        public void FindNode(long entityGid)
        {
            // Schema view has not been opened
            if (SchemaViewModel == null)
            {
                long neededSubstationGid = GetSubstationGidForEntity(entityGid);
                if (neededSubstationGid == 0)
                {
                    return;
                }

                FetchAndOpenSchema(neededSubstationGid, entityGid);
                return;
            }

            // Entity is already on shown schema
            if (SchemaViewModel.Locate(entityGid))
            {
                return;
            }

            // Find subnetwork containing specified entity
            long substationGid = GetSubstationGidForEntity(entityGid);
            if (substationGid == 0)
            {
                return;
            }

            SelectedEnergySource = EnergySources.First(x => x.GlobalId == substationGid);
            ExecuteGetSchemaCommand(null);
            SchemaViewModel.Locate(entityGid);
        }

        protected void FetchEnergySources(object sender, ElapsedEventArgs e)
        {
            List<EnergySourceBrowseSchemaItem> entities = GetEntitiesFromService();

            if (entities?.Count == 0)
            {
                return;
            }

            try
            {
                Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (Action)(() => UpdateSubstations(entities)));
            }
            catch { }
        }

        private void FetchAndOpenSchema(long substationGid, long locatedEntityGid)
        {
            List<EnergySourceBrowseSchemaItem> entities = GetEntitiesFromService();

            if (entities?.Count == 0)
            {
                return;
            }

            UpdateSubstations(entities);

            SelectedEnergySource = EnergySources.First(x => x.GlobalId == substationGid);
            ExecuteGetSchemaCommand(null);
            SchemaViewModel.Locate(locatedEntityGid);
        }

        private void ExecuteClearSearchingNodeCommand(object param)
        {
            if (SchemaViewModel.LocatedNode != null)
            {
                SchemaViewModel.LocatedNode.Located = false;
                SchemaViewModel.LocatedNode = null;
            }
        }

        private bool CanClearSearchingNodeCommandExecute(object param)
        {
            if (SchemaViewModel == null)
            {
                return false;
            }

            if (SchemaViewModel.LocatedNode != null)
            {
                return true;
            }

            return false;
        }

        private List<EnergySourceBrowseSchemaItem> GetEntitiesFromService()
        {
            List<EnergySourceDTO> items;
            try
            {
                items = schemaClient.Proxy.GetSubstations();
            }
            catch
            {
                items = new List<EnergySourceDTO>();
            }

            List<EnergySourceBrowseSchemaItem> summaryItems = new List<EnergySourceBrowseSchemaItem>(items.Count);

            foreach (var item in items)
            {
                EnergySourceBrowseSchemaItem summaryItem = new EnergySourceBrowseSchemaItem();
                summaryItem.Update(item);

                summaryItems.Add(summaryItem);
            }

            return summaryItems;
        }

        private void UpdateSubstations(List<EnergySourceBrowseSchemaItem> entities)
        {
            foreach (var entity in entities)
            {
                EnergySources.AddOrUpdateEntity(entity);
            }
        }

        private bool CanGetSchemaCommandExecute(object parameter)
        {
            if (selectedEnergySource == null)
            {
                return false;
            }

            return selectedEnergySource.GlobalId != SchemaViewModel?.EnergySourceGlobalId;
        }

        private void ExecuteGetSchemaCommand(object parameter)
        {
            SubSchemaDTO newSchema = schemaClient.Proxy.GetSchema(selectedEnergySource.GlobalId);
            SchemaEnergyBalanceDTO energyBalance = schemaClient.Proxy.GetEnergyBalance(selectedEnergySource.GlobalId);

            SchemaGraphWrapper graphWrapper = schemaCreator.CreateSchema(newSchema);

            ShowNewSchema(graphWrapper, energyBalance);
        }

        private void ShowNewSchema(SchemaGraphWrapper graphWrapper, SchemaEnergyBalance energyBalance)
        {
            SchemaViewModel?.StopProcessingGraph();

            SchemaViewModel = new SchemaViewModel(graphWrapper, energyBalance, schemaClient);
        }

        private long GetSubstationGidForEntity(long entityGid)
        {
            try
            {
                return schemaClient.Proxy.SubStationContainsEntity(entityGid);
            }
            catch
            {
                return 0;
            }
        }
    }
}
