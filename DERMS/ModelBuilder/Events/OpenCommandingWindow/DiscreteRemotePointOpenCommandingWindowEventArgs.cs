namespace ClientUI.Events.OpenCommandingWindow
{
    public class DiscreteRemotePointOpenCommandingWindowEventArgs : OpenCommandingWindowEventArgs
    {
        public string Name { get; set; }

        public int Address { get; set; }

        public int Value { get; set; }

        public int NormalValue { get; set; }
    }
}
