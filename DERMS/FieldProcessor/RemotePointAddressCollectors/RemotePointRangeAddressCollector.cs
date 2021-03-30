using FieldProcessor.Model;
using System.Collections.Generic;
using System.Linq;

namespace FieldProcessor.RemotePointAddressCollector
{
    public class AddressRange
    {
        public AddressRange(ushort startAddress)
        {
            StartAddress = startAddress;
            RangeSize = 1;
        }

        public ushort StartAddress { get; private set; }
        public ushort RangeSize { get; private set; }

        public void IncrementAddressRange()
        {
            RangeSize++;
        }
    }

    public class RemotePointRangeAddressCollector : RemotePointAddressCollector, IRemotePointRangeAddressCollector
    {
        private readonly int maximumNumberOfAnalogPointsInArray = 62; // ((2000 bits / 8)/2)/2 => 4 byte values
        private readonly int maximumNumberOfDiscretePointsInArray = 2000;

        private Dictionary<RemotePointType, List<AddressRange>> addressRanges;

        public RemotePointRangeAddressCollector()
        {
            addressRanges = new Dictionary<RemotePointType, List<AddressRange>>()
            {
                { RemotePointType.Coil, new List<AddressRange>() },
                { RemotePointType.DiscreteInput, new List<AddressRange>() },
                { RemotePointType.HoldingRegister, new List<AddressRange>() },
                { RemotePointType.InputRegister, new List<AddressRange>() },
            };
        }

        public override void Commit()
        {
            locker.AcquireWriterLock(lockerTimeout);

            foreach (var pair in transactionCopy)
            {
                addressRanges[pair.Key] = CreateAddressRanges(pair.Key, pair.Value.Values.ToList());
            }

            locker.ReleaseWriterLock();
        }

        public List<AddressRange> GetAddressRanges(RemotePointType remotePointType)
        {
            List<AddressRange> addressRanges;

            locker.AcquireReaderLock(lockerTimeout);

            addressRanges = this.addressRanges[remotePointType];

            locker.ReleaseReaderLock();

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
