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

namespace ClientUI.ViewModels.Summaries.DERGroupSummary
{
    public class DERGroupSummaryViewModel : CommandingSummaryViewModel<DERGroupSummaryItem, DERGroupOpenCommandingWindowEvent, DERGroupOpenCommandingWindowEventArgs>
    {
        private WCFClient<IDERGroupSummaryJob> summaryJob;

        public DERGroupSummaryViewModel() : base("DERGroup Summary", ContentType.DERGroupSummary)
        {
            summaryJob = new WCFClient<IDERGroupSummaryJob>("uiAdapterDERGroupEndpoint");

            DataGridItemDoubleClicked = new RelayCommand(DataGridItemSelected, null);
        }

        public ICommand DataGridItemDoubleClicked { get; set; }

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

        private void DataGridItemSelected(object item)
        {
            if (item == null)
            {
                return;
            }

            RaiseEventForCommandingWindow(item as DERGroupSummaryItem);
        }
    }
}
