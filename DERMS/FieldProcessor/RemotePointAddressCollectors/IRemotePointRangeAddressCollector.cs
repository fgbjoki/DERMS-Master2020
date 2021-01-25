using System.Collections.Generic;
using FieldProcessor.Model;

namespace FieldProcessor.RemotePointAddressCollector
{
    public interface IRemotePointRangeAddressCollector
    {
        List<AddressRange> GetAddressRanges(RemotePointType remotePointType);
    }
}