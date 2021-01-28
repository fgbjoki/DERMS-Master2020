using Common.UIDataTransferObject.RemotePoints;
using ClientUI.SummaryCreator;
using ClientUI.Common;
using System.Windows.Input;
using ClientUI.Events.OpenCommandingWindow;
using System;

namespace ClientUI.ViewModels.Summaries.RemotePointSummaries
{
    public class AnalogRemotePointSummaryViewModel : CommandingSummaryViewModel<AnalogRemotePointSummaryDTO, AnalogRemotePointOpenCommandingWindowEvent, AnalogRemotePointOpenCommandingWindowEventArgs>
    {
        public AnalogRemotePointSummaryViewModel() : base("Analog Remote Point Summary", ContentType.AnalogRemotePointSummary)
        {
            // TODO REMOVE THIS, GET ITEMS FROM UIADAPTER INSTEAD
            CreateSampleItems();

            DataGridItemDoubleClicked = new RelayCommand(DataGridItemSelected, null);
        }

        private void CreateSampleItems()
        {
            Items.Add(new AnalogRemotePointSummaryDTO() { GlobalId = 1, Name = "AnalogTest1", Address = 1, Value = 2 });
            Items.Add(new AnalogRemotePointSummaryDTO() { GlobalId = 2, Name = "BrzaTacka", Address = 3, Value = 22 });
            Items.Add(new AnalogRemotePointSummaryDTO() { GlobalId = 3, Name = "Spora Tacka", Address = 11, Value = 10 });
            Items.Add(new AnalogRemotePointSummaryDTO() { GlobalId = 4, Name = "Wauuuuuuu", Address = 51, Value = 51 });
        }

        public ICommand DataGridItemDoubleClicked { get; set; }

        protected override AnalogRemotePointOpenCommandingWindowEventArgs TransformGridDataToEventArg(AnalogRemotePointSummaryDTO selectedItem)
        {
            return new AnalogRemotePointOpenCommandingWindowEventArgs()
            {
                GlobalId = selectedItem.GlobalId,
                Address = selectedItem.Address,
                Name = selectedItem.Name,
                Value = selectedItem.Value
            };
        }

        private void DataGridItemSelected(object item)
        {
            if (item == null)
            {
                return;
            }

            RaiseEventForCommandingWindow(item as AnalogRemotePointSummaryDTO);
        }
    }
}
