using FieldProcessor.Model;
using FieldProcessor.RemotePointAddressCollector;

namespace FieldProcessor.ValueExtractor
{
    public class AnalogReadCommandExtractValueProcessor : ReadCommandExtractValueProcessor
    {
        public AnalogReadCommandExtractValueProcessor(IFieldValueReader fieldValueReader, RemotePointType remotePointType, IRemotePointSortedAddressCollector remotePointAddressCollector) : base(fieldValueReader, remotePointType, remotePointAddressCollector)
        {
        }

        protected override void MoveAddressCounter(ref int counter)
        {
            counter += 2;
        }
    }
}
