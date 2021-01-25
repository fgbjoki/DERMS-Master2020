using System.Collections.Generic;

namespace FieldProcessor.ValueExtractor
{
    public class BitFieldValueReader : IFieldValueReader
    {
        public List<int> CreateValueCollection(ushort quantity, byte[] values)
        {
            List<int> readValues = new List<int>(quantity);

            for (int i = 0; i < quantity; i++)
            {
                readValues.Add((values[i / 8] >> (i % 8)) & 1);
            }

            return readValues;
        }
    }
}
