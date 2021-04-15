namespace FieldSimulator.Model
{
    public delegate void DiscretePointValueChanged(RemotePointType pointType, ushort address, short value);

    public class DiscretePointWrapper : BasePoint
    {
        private short value;

        public DiscretePointWrapper(RemotePointType pointType, int index) : base(pointType, index)
        {
        }

        public short Value
        {
            get { return value; }
            set
            {
                if (this.value == value)
                {
                    return;
                }

                base.SetProperty(ref this.value, value);
                DiscretePointValueChanged.Invoke(pointType, address, value);
            }
        }

        public event DiscretePointValueChanged DiscretePointValueChanged = delegate { };
    }
}
