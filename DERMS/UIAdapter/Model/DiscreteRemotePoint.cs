namespace UIAdapter.Model
{
    public class DiscreteRemotePoint : RemotePoint
    {
        private int value;

        public DiscreteRemotePoint(long globalId) : base(globalId)
        {
        }

        public int Value
        {
            get { return value; }
            set
            {
                this.value = value;
                PopulateValueField(value);
            }
        }

        public int NormalValue { get; set; }
    }
}
