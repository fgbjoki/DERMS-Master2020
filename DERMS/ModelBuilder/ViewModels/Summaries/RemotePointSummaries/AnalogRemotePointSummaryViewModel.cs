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
            summaryJob = new WCFClient<IAnalogRemotePointSummaryJob>("uiAdapterEndpoint");

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

        protected override List<AnalogRemotePointSummaryDTO> GetEntitiesFromService()
        {
            List<AnalogRemotePointSummaryDTO> items;
            try
            {
                items = summaryJob.Proxy.GetAllEntities();
            }
            catch
            {
                items = new List<AnalogRemotePointSummaryDTO>();
            }

            // TODO CONVERT FROM DTO TO SUMMARYITEM

            return items;
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