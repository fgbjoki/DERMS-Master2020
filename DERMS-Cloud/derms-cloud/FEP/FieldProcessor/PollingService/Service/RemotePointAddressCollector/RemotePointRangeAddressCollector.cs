using Core.Common.Transaction.Models.FEP.FEPStorage;
using System.Collections.Generic;
using System.Linq;

namespace PollingService.Service.RemotePointAddressCollector
{
    public class RemotePointRangeAddressCollector : IRemotePointRangeAddressCollector
    {
        private readonly int maximumNumberOfAnalogPointsInArray = 31; // ((2000 bits / 8)/2)/2 / 2 => 8 byte values
        private readonly int maximumNumberOfDiscretePointsInArray = 2000;

        public Dictionary<RemotePointType, List<AddressRange>> CreateAddressRanges(List<RemotePoint> remotePoints)
        {
            Dictionary<RemotePointType, List<AddressRange>> addressRanges = new Dictionary<RemotePointType, List<AddressRange>>()
            {
                { RemotePointType.Coil, new List<AddressRange>() },
                { RemotePointType.DiscreteInput, new List<AddressRange>() },
                { RemotePointType.HoldingRegister, new List<AddressRange>() },
                { RemotePointType.InputRegister, new List<AddressRange>() },
            };

            Dictionary<RemotePointType, List<RemotePoint>> sortedRemotePoints = remotePoints.GroupBy(x => x.Type).ToDictionary(x => x.Key, y => y.ToList());
            foreach (var remotePointType in sortedRemotePoints)
            {
                addressRanges[remotePointType.Key] = CreateAddressRanges(remotePointType.Key, remotePointType.Value);
            }

            return addressRanges;
        }

        private List<AddressRange> CreateAddressRanges(RemotePointType pointType, List<RemotePoint> remotePoints)
        {
            if (remotePoints?.Count == 0)
            {
                return new List<AddressRange>();
            }

            List<AddressRange> addressRanges = new List<AddressRange>();

            AddressRange addressRange = null;
            for (int i = 0; i < remotePoints.Count; i++)
            {
                if (ShouldCreateNewAddressRange(pointType, addressRange) || IsAddressRangeWithGap(remotePoints[i].Address, addressRange.StartAddress + i))
                {
                    addressRange = new AddressRange(remotePoints[i].Address);
                    addressRanges.Add(addressRange);
                }
                else
                {
                    addressRange.IncrementAddressRange();
                }
            }

            return addressRanges;
        }

        private static bool IsAddressRangeWithGap(ushort remotePointAddress, int currentRangeAddress)
        {
            return remotePointAddress > currentRangeAddress;
        }

        private bool ShouldCreateNewAddressRange(RemotePointType pointType, AddressRange addressRange)
        {
            return addressRange == null || addressRange.RangeSize == GetMaximumConsecutiveAddressesForRemotePoint(pointType);
        }

        private int GetMaximumConsecutiveAddressesForRemotePoint(RemotePointType remotePointType)
        {
            switch (remotePointType)
            {
                case RemotePointType.Coil:
                case RemotePointType.DiscreteInput:
                    return maximumNumberOfDiscretePointsInArray;
                case RemotePointType.HoldingRegister:
                case RemotePointType.InputRegister:
                    return maximumNumberOfAnalogPointsInArray;
                default:
                    return 0;
            }
        }
    }
}
