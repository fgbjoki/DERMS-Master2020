using System.Collections.Generic;
using FieldProcessor.Model;

namespace FieldProcessor.ValueExtractor
{
    public interface IRemotePointAddressCollector
    {
        List<RemotePoint> GetSortedAddresses(RemotePointType pointType);
    }
}