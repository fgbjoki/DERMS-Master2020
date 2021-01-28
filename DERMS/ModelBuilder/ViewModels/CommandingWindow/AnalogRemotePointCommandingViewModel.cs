using ClientUI.Events.OpenCommandingWindow;

namespace ClientUI.ViewModels.CommandingWindow
{
    public class AnalogRemotePointCommandingViewModel : CommandingViewModel
    {
        public AnalogRemotePointCommandingViewModel(AnalogRemotePointOpenCommandingWindowEventArgs analogRemotePoint) : base("Analog Remote Point Commanding Window")
        {
            GlobalId = analogRemotePoint.GlobalId;
            Name = analogRemotePoint.Name;
            Address = analogRemotePoint.Address;
            Value = analogRemotePoint.Value;
        }

        public long GlobalId { get; set; }

        public string Name { get; set; }

        public int Address { get; set; }

        public float Value { get; set; }
    }
}
