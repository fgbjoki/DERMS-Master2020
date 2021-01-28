namespace ClientUI.Events.OpenCommandingWindow
{
    public class AnalogRemotePointOpenCommandingWindowEventArgs : OpenCommandingWindowEventArgs
    {
        public string Name { get; set; }

        public int Address { get; set; }

        public float Value { get; set; }
    }
}
