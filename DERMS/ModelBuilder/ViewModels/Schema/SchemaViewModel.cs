using ClientUI.Models.Schema;
using Common.Communication;
using Common.ServiceInterfaces.UIAdapter;
using Common.UIDataTransferObject.Schema;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace ClientUI.ViewModels.Schema
{
    public class SchemaViewModel : BaseViewModel
    {
        private WCFClient<ISchema> schemaClient;

        private Dictionary<long, SchemaNode> schemaNodes;
        private Timer fetchNewDataTimer;

        public SchemaViewModel(SchemaGraphWrapper graphWrapper, WCFClient<ISchema> schemaClient)
        {
            this.schemaClient = schemaClient;

            Nodes = new ObservableCollection<SchemaNode>();

            InitializeGraph(graphWrapper);

            InitializeTimer();
        }

        private void InitializeTimer()
        {
            fetchNewDataTimer = new Timer();
            fetchNewDataTimer.AutoReset = true;
            fetchNewDataTimer.Interval = 1000 * 4;
            fetchNewDataTimer.Enabled = true;
            fetchNewDataTimer.Elapsed += FetchNewDataTimer_Elapsed;
        }

        public ObservableCollection<SchemaNode> Nodes { get; set; }

        public long EnergySourceGlobalId { get; set; }

        public void StopProcessingGraph()
        {
            fetchNewDataTimer.Enabled = false;
        }

        private void FetchNewDataTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (EnergySourceGlobalId == 0)
            {
                return;
            }

            var equipmentStateDTO = schemaClient.Proxy.GetEquipmentStates(EnergySourceGlobalId);

            Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (Action)(() => UpdateNodes(equipmentStateDTO)));
        }

        private void UpdateNodes(SubSchemaConductingEquipmentEnergized currentNodeStates)
        {
            foreach (var newNodeState in currentNodeStates.Nodes)
            {
                SchemaNode schemaNode;
                if (schemaNodes.TryGetValue(newNodeState.Key, out schemaNode))
                {
                    schemaNode.DoesConduct = newNodeState.Value.DoesConduct;
                    schemaNode.Energized = newNodeState.Value.IsEnergized;
                }
            }
        }

        private void InitializeGraph(SchemaGraphWrapper graphWrapper)
        {
            Nodes.Add(graphWrapper.Root);
            schemaNodes = graphWrapper.Nodes;

            EnergySourceGlobalId = graphWrapper.Root.GlobalId;
        }
    }
}
