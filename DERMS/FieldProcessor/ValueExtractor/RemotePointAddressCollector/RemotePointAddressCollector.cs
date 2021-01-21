using FieldProcessor.Model;
using System.Collections.Generic;
using System.Threading;

namespace FieldProcessor.ValueExtractor
{
    public class RemotePointAddressCollector : IRemotePointAddressCollector
    {
        private static readonly int lockerTimeout = 2000;

        private Dictionary<RemotePointType, List<RemotePoint>> sortedAddressesPerRemotePointType;
        private Dictionary<RemotePointType, SortedList<ushort, RemotePoint>> transactionCopy;

        private ReaderWriterLock locker;

        public RemotePointAddressCollector()
        {
            locker = new ReaderWriterLock();
        }

        public void Prepare(List<RemotePoint> remotePoints)
        {
            if (transactionCopy == null)
            {
                transactionCopy = new Dictionary<RemotePointType, SortedList<ushort, RemotePoint>>()
                {
                    { RemotePointType.Coil, new SortedList<ushort, RemotePoint>() },
                    { RemotePointType.DiscreteInput, new SortedList<ushort, RemotePoint>() },
                    { RemotePointType.HoldingRegister, new SortedList<ushort, RemotePoint>() },
                    { RemotePointType.InputRegister, new SortedList<ushort, RemotePoint>() },
                };
            }

            foreach (var remotePoint in remotePoints)
            {               
                transactionCopy[remotePoint.Type].Add(remotePoint.Address, remotePoint);
            }
        }

        public void Commit()
        {
            locker.AcquireWriterLock(lockerTimeout);

            foreach (var pair in transactionCopy)
            {
                sortedAddressesPerRemotePointType[pair.Key] = new List<RemotePoint>(pair.Value.Values);
            }

            transactionCopy = null;

            locker.ReleaseWriterLock();
        }

        public void Rollback()
        {
            transactionCopy = null;
        }

        public List<RemotePoint> GetSortedAddresses(RemotePointType pointType)
        {
            List<RemotePoint> remotePoints;

            locker.AcquireReaderLock(lockerTimeout);

            remotePoints = sortedAddressesPerRemotePointType[pointType];

            locker.ReleaseReaderLock();

            return remotePoints;
        }
    }
}
