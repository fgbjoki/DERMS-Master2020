using ClientUI.Common;
using ClientUI.Events.OpenCommandingWindow;
using ClientUI.Models.Schema.Nodes;
using ClientUI.SummaryCreator;
using Common.UIDataTransferObject.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ClientUI.Models.Schema.NodeCreators
{
    public class EnergyStorageSchemaNodeCreator : SchemaNodeCreator
    {
        public EnergyStorageSchemaNodeCreator() : base("../../Resources/storageSchema.png")
        {
        }

        protected override ICommand GetOnClickCommand()
        {
            return new RelayCommand(OpenCommandingWindow);
        }

        private void OpenCommandingWindow(object param)
        {
            long gid = (long)param;
            SummaryManager.Instance.EventAggregator.GetEvent<DERGroupOpenCommandingWindowEvent>().Publish(new DERGroupOpenCommandingWindowEventArgs()
            {
                DERView = ViewModels.CommandingWindow.DERGroup.DERView.BatteryView,
                GlobalId = gid
            });
        }
    }
}
