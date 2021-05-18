using ClientUI.Events.OpenCommandingWindow;
using ClientUI.Models.RemotePoints;
using Common.Communication;
using Common.ServiceInterfaces.UIAdapter.SummaryJobs;
using Common.UIDataTransferObject.RemotePoints;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Timers;
using System.Windows.Input;
using ClientUI.SummaryCreator;
using ClientUI.Common;

namespace ClientUI.ViewModels.Summaries.RemotePointSummaries
{
    public class DiscreteRemotePointSummaryViewModel : CommandingSummaryViewModel<DiscreteRemotePointSummaryItem, DiscreteRemotePointOpenCommandingWindowEvent, DiscreteRemotePointOpenCommandingWindowEventArgs>
    {
        private WCFClient<IDiscreteRemotePointSummaryJob> summaryJob;

        public DiscreteRemotePointSummaryViewModel() : base("Discrete Remote Point Summary", ContentType.DiscreteRemotePointSummary)
        {
            summaryJob = new WCFClient<IDiscreteRemotePointSummaryJob>("uiAdapterEndpointDiscrete");

            DataGridItemDoubleClicked = new RelayCommand(DataGridItemSelected, null);
        }

        public ICommand DataGridItemDoubleClicked { get; set; }

        private void DataGridItemSelected(object item)
        {
            if (item == null)
            {
                return;
            }

            RaiseEventForCommandingWindow(item as DiscreteRemotePointSummaryItem);
        }

        protected override List<DiscreteRemotePointSummaryItem> GetEntitiesFromService()
        {
            List<DiscreteRemotePointSummaryDTO> items;
            try
            {
                items = summaryJob.Proxy.GetAllDiscreteEntities();
            }
            catch
            {
                items = new List<DiscreteRemotePointSummaryDTO>();
            }

            List<DiscreteRemotePointSummaryItem> summaryItems = new List<DiscreteRemotePointSummaryItem>(items.Count);

            foreach (var item in items)
            {
                DiscreteRemotePointSummaryItem summaryItem = new DiscreteRemotePointSummaryItem();
                summaryItem.Update(item);

                summaryItems.Add(summaryItem);
            }

            return summaryItems;
        }

        protected override DiscreteRemotePointOpenCommandingWindowEventArgs TransformGridDataToEventArg(DiscreteRemotePointSummaryItem selectedItem)
        {
            return new DiscreteRemotePointOpenCommandingWindowEventArgs()
            {
                GlobalId = selectedItem.GlobalId,
                Address = selectedItem.Address,
                Name = selectedItem.Name,
                Value = selectedItem.Value,
                NormalValue = selectedItem.NormalValue
            };
        }
    }
}
