using System.Collections.Generic;
using FieldProcessor.Model;

namespace FieldProcessor.RemotePointAddressCollector
{
    public interface IRemotePointSortedAddressCollector
    {
        List<RemotePoint> GetSortedAddresses(RemotePointType pointType);
    }
}