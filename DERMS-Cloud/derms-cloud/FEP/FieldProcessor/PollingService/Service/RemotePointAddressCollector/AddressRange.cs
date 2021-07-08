namespace PollingService.Service.RemotePointAddressCollector
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
}
