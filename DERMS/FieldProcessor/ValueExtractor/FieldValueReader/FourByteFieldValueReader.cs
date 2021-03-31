using System.Collections.Generic;

namespace FieldProcessor.ValueExtractor
{
    public class FourByteFieldValueReader : TwoByteFieldValueReader
    {
        public override List<int> CreateValueCollection(ushort quantity, byte[] values)
        {
            List<int> joinedValues = new List<int>(quantity/2);
            List<int> twoBytedValues = base.CreateValueCollection(quantity, values);

            for (int i = 0; i < twoBytedValues.Count; i += 2)
            {
                int joinedValue = JoinValues((ushort)twoBytedValues[i], (ushort)twoBytedValues[i + 1]);
                joinedValues.Add(joinedValue);
            }

            return joinedValues;
        }

        private int JoinValues(ushort first, ushort second)
        {
            unsafe
            {
                int joinedValue;
                ushort* iterator = (ushort*)&joinedValue;

                *(iterator+ 1) = first;
                *iterator = second;

                return joinedValue;
            }
        }
    }
}
