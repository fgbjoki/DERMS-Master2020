using Common.UIDataTransferObject.RemotePoints;
using ClientUI.SummaryCreator;
using ClientUI.Common;
using System.Windows.Input;
using ClientUI.Events.OpenCommandingWindow;
using System;
using System.Timers;
using Common.Communication;
using Common.ServiceInterfaces.UIAdapter.SummaryJobs;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using ClientUI.Models.RemotePoints;

namespace ClientUI.ViewModels.Summaries.RemotePointSummaries
{
    public class AnalogRemotePointSummaryViewModel : CommandingSummaryViewModel<AnalogRemotePointSummaryItem, AnalogRemotePointSummaryDTO, AnalogRemotePointOpenCommandingWindowEvent, AnalogRemotePointOpenCommandingWindowEventArgs>
    {
        private WCFClient<IAnalogRemotePointSummaryJob> summaryJob;

        public AnalogRemotePointSummaryViewModel() : base("Analog Remote Point Summary", ContentType.AnalogRemotePointSummary)
        {
            summaryJob = new WCFClient<IAnalogRemotePointSummaryJob>("uiAdapterEndpointAnalog");

            DataGridItemDoubleClicked = new RelayCommand(DataGridItemSelected, null);
        }

        public ICommand DataGridItemDoubleClicked { get; set; }

        private void DataGridItemSelected(object item)
        {
            if (item == null)
            {
                return;
            }

            RaiseEventForCommandingWindow(item as AnalogRemotePointSummaryItem);
        }

        protected override List<AnalogRemotePointSummaryItem> GetEntitiesFromService()
        {
            List<AnalogRemotePointSummaryDTO> items;
            try
            {
                items = summaryJob.Proxy.GetAllAnalogEntities();
            }
            catch
            {
                items = new List<AnalogRemotePointSummaryDTO>();
            }

            List<AnalogRemotePointSummaryItem> summaryItems = new List<AnalogRemotePointSummaryItem>(items.Count);

            foreach (var item in items)
            {
                AnalogRemotePointSummaryItem summaryItem = new AnalogRemotePointSummaryItem();
                summaryItem.Update(item);

                summaryItems.Add(summaryItem);
            }

            return summaryItems;
        }

        protected override AnalogRemotePointOpenCommandingWindowEventArgs TransformGridDataToEventArg(AnalogRemotePointSummaryItem selectedItem)
        {
            return new AnalogRemotePointOpenCommandingWindowEventArgs()
            {
                GlobalId = selectedItem.GlobalId,
                Address = selectedItem.Address,
                Name = selectedItem.Name,
                Value = selectedItem.Value
            };
        }
    }
}