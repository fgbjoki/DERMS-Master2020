using System;
using System.Collections.Generic;
using System.Net;

namespace FieldProcessor.ValueExtractor
{
    public class TwoByteFieldValueReader : IFieldValueReader
    {
        public virtual List<int> CreateValueCollection(ushort quantity, byte[] values)
        {
            List<int> readValues = new List<int>(quantity);

            for (int i = 0; i < quantity; i++)
            {
                ushort value = (ushort)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(values, i * 2));
                readValues.Add(value);
            }

            return readValues;
        }
    }
}
