using System.Collections.Generic;
using FieldProcessor.Model;

namespace FieldProcessor.RemotePointAddressCollector
{
    public class RemotePointSortedAddressCollector : RemotePointAddressCollector, IRemotePointSortedAddressCollector
    {
        private Dictionary<RemotePointType, List<RemotePoint>> sortedAddresses;

        public RemotePointSortedAddressCollector() : base()
        {
            sortedAddresses = new Dictionary<RemotePointType, List<RemotePoint>>();
        }

        public override void Commit()
        {
            locker.AcquireWriterLock(lockerTimeout);

            foreach (var pair in transactionCopy)
            {
                sortedAddresses[pair.Key] = new List<RemotePoint>(pair.Value.Values);
            }

            locker.ReleaseWriterLock();
        }

        public List<RemotePoint> GetSortedAddresses(RemotePointType pointType)
        {
            List<RemotePoint> remotePoints;

            locker.AcquireReaderLock(lockerTimeout);

            remotePoints = sortedAddresses[pointType];

            locker.ReleaseReaderLock();

            return remotePoints;
        }
    }
}
