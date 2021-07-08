using Core.Common.Transaction.Models.FEP.FEPStorage;
using System.Collections.Generic;

namespace PollingService.Service.RemotePointAddressCollector
{
    public interface IRemotePointRangeAddressCollector
    {
        Dictionary<RemotePointType, List<AddressRange>> CreateAddressRanges(List<RemotePoint> remotePoints);
    }
}