using System;

namespace FieldSimulator.Model
{
    public class AnalogPointWrapper : BasePoint
    {
        private float floatValue;

        public AnalogPointWrapper(PointType pointType, int index) : base(pointType, index)
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

                var splitValue = SplitValue(floatValue);
                PublishChangedValue(index, splitValue.Item1);
                PublishChangedValue(index + 1, splitValue.Item2);
            }
        }

        private Tuple<short, short> SplitValue(float floatValue)
        {
            unsafe
            {
                short* iterator = (short*)(&floatValue);
                short firstPart = *iterator++;
                short secondPart = *iterator;

                return new Tuple<short, short>(firstPart, secondPart);
            }
        }

        /// <summary>
        /// Change one of the register value. If the indexes are the same first register is targeted, otherwise second register is targeted.
        /// </summary>
        /// <param name="value">Value to be set.</param>
        /// <param name="index">Identifies the register.</param>
        public virtual void ChangeValue(short value, int index)
        {
            unsafe
            {
                if (this.index == index)
                {
                    SetPartOfValue(0, value);
                }
                else if (this.index + 1 == index)
                {
                    SetPartOfValue(1, value);
                }
            }
        }

        private void SetPartOfValue(int part, short value)
        {
            float tempValue;
            unsafe
            {                
                short* iterator = (short*)&tempValue;

                iterator += part;

                *iterator = value;
            }

            FloatValue = tempValue;
        }
    }
}
