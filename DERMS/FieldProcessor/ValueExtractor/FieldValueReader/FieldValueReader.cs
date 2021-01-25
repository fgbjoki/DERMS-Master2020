using System.Collections.Generic;

namespace FieldProcessor.ValueExtractor
{
    public interface IFieldValueReader
    {
        List<int> CreateValueCollection(ushort quantity, byte[] values);
    }
}
