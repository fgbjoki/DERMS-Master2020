using ClientUI.Common;
using ClientUI.CustomControls.PieChart;
using ClientUI.Models.Schema;
using ClientUI.Models.Schema.Nodes;
using Common.AbstractModel;
using Common.Communication;
using Common.ServiceInterfaces.UIAdapter;
using Common.UIDataTransferObject.Schema;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;

namespace ClientUI.ViewModels.Schema
{
    public class SchemaViewModel : BaseViewModel
    {
        private WCFClient<ISchema> schemaClient;

        private Dictionary<long, SchemaNode> schemaNodes;
        private SchemaEnergyBalance energyBalance;
        private Timer fetchNewDataTimer;

        private SchemaNode locatedNode;

        public SchemaViewModel(SchemaGraphWrapper graphWrapper, SchemaEnergyBalance energyBalance, WCFClient<ISchema> schemaClient)
        {
            this.schemaClient = schemaClient;
            this.energyBalance = energyBalance;

            Nodes = new ObservableCollection<SchemaNode>();

            InitializePieCharts();

            InitializeGraph(graphWrapper);

            InitializeTimer();
        }

        public ObservableCollection<SchemaNode> Nodes { get; set; }

        public long EnergySourceGlobalId { get; set; }

        public SchemaEnergyBalance EnergyBalance { get { return energyBalance; } }

        public ResponsePieChartViewModel ResponseViewModel { get; set; }

        public ProductionPerTypePieChartViewModel ProductionPerTypeViewModel { get; set; }

        public void StopProcessingGraph()
        {
            fetchNewDataTimer.Enabled = false;
        }

        public bool Locate(long wantedEntityGid)
        {
            if (LocatedNode != null)
            {
                LocatedNode.Located = false;
            }

            SchemaNode locatedNode;
            if (!schemaNodes.TryGetValue(wantedEntityGid, out locatedNode))
            {
                return false;
            }

            LocatedNode = locatedNode;
            LocatedNode.Located = true;

            return true;
        }

        public SchemaNode LocatedNode
        {
            get { return locatedNode; }
            set { SetProperty(ref locatedNode, value); }
        }

        private void FetchNewDataTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (EnergySourceGlobalId == 0)
            {
                return;
            }

            fetchNewDataTimer.Enabled = false;

            var equipmentStateDTO = schemaClient.Proxy.GetEquipmentStates(EnergySourceGlobalId);
            var energyBalanceDTO = schemaClient.Proxy.GetEnergyBalance(EnergySourceGlobalId);

            Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (Action)(() => Update(equipmentStateDTO, energyBalanceDTO)));

            fetchNewDataTimer.Enabled = true;
        }

        private void Update(SubSchemaConductingEquipmentEnergized currentNodeStates, SchemaEnergyBalanceDTO energyBalance)
        {
            PopulateEnergyBalance(energyBalance);

            foreach (var newNodeState in currentNodeStates.Nodes.Values)
            {
                PopulateNodesWithNewStates(newNodeState);
            }
        }

        private void PopulateEnergyBalance(SchemaEnergyBalanceDTO energyBalance)
        {
            EnergyBalance.DemandEnergy = energyBalance.DemandEnergy;
            ResponseViewModel.Imported = EnergyBalance.ImportedEnergy = energyBalance.ImportedEnergy;
            ResponseViewModel.DERs = EnergyBalance.ProducedEnergy = energyBalance.ProducedEnergy;

            ProductionPerTypeViewModel.SolarEnergyProduced = energyBalance.SolarEnergyProduction;
            ProductionPerTypeViewModel.WindEnergyProduced = energyBalance.WindEnergyProduction;
            ProductionPerTypeViewModel.EnergyStorageEnergyProduced = energyBalance.EnergyStorageEnergyProduction;
        }

        private void PopulateNodesWithNewStates(SubSchemaNodeDTO dtoNode)
        {
            SchemaNode schemaNode;
            if (schemaNodes.TryGetValue(dtoNode.GlobalId, out schemaNode))
            {
                schemaNode.DoesConduct = dtoNode.DoesConduct;
                schemaNode.Energized = dtoNode.IsEnergized;
            }

            if (schemaNode.DMSType == DMSType.BREAKER)
            {
                SchemaBreakerNode breakerNode = schemaNode as SchemaBreakerNode;
                SubSchemaBreakerNodeDTO breakerNodeDto = dtoNode as SubSchemaBreakerNodeDTO;
                breakerNode.Closed = breakerNodeDto.Closed;
            }
        }

        private void InitializeGraph(SchemaGraphWrapper graphWrapper)
        {
            Nodes.Add(graphWrapper.Root);
            schemaNodes = graphWrapper.Nodes;

            EnergySourceGlobalId = graphWrapper.Root.GlobalId;
        }

        private void InitializeTimer()
        {
            fetchNewDataTimer = new Timer();
            fetchNewDataTimer.AutoReset = true;
            fetchNewDataTimer.Interval = 1000 * 4;
            fetchNewDataTimer.Enabled = true;
            fetchNewDataTimer.Elapsed += FetchNewDataTimer_Elapsed;
        }

        private void InitializePieCharts()
        {
            ResponseViewModel = new ResponsePieChartViewModel();
            ResponseViewModel.Imported = energyBalance.ImportedEnergy;
            ResponseViewModel.DERs = energyBalance.ProducedEnergy;

            ProductionPerTypeViewModel = new ProductionPerTypePieChartViewModel();
            ProductionPerTypeViewModel.SolarEnergyProduced = energyBalance.SolarEnergyProduced;
            ProductionPerTypeViewModel.WindEnergyProduced = energyBalance.WindEnergyProduced;
            ProductionPerTypeViewModel.EnergyStorageEnergyProduced = energyBalance.EnergyStorageEnergyProduced;
        }
    }
}
