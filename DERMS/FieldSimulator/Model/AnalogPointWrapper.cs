namespace FieldSimulator.Model
{
    public delegate void AnalogPointValueChanged(RemotePointType pointType, int index, float value);

    public class AnalogPointWrapper : BasePoint
    {
        private float floatValue;

        public AnalogPointWrapper(RemotePointType pointType, int index) : base(pointType, index)
        {
        }

        public float FloatValue
        {
            get { return floatValue; }
            set
            {
                if (floatValue == value)
                {
                    return;
                }

                SetProperty(ref floatValue, value);
                AnalogPointValueChanged.Invoke(pointType, address, value);              
            }
        }

        public event AnalogPointValueChanged AnalogPointValueChanged = delegate { };
    }
}
