using FieldProcessor.Model;
using FieldProcessor.TransactionProcessing.TransactionProcessors;
using System.Collections.Generic;
using System.Threading;
using System;

namespace FieldProcessor.RemotePointAddressCollector
{
    public abstract class RemotePointAddressCollector : IRemotePointDependentTransactionUnit
    {
        protected static readonly int lockerTimeout = 2000;

        protected Dictionary<RemotePointType, List<RemotePoint>> addressCollection;
        protected Dictionary<RemotePointType, SortedList<ushort, RemotePoint>> transactionCopy;

        protected ReaderWriterLock locker;

        protected RemotePointAddressCollector()
        {
            locker = new ReaderWriterLock();
        }

        public bool Prepare(List<RemotePoint> remotePoints)
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
                if (remotePoint.Type == RemotePointType.Coil || remotePoint.Type == RemotePointType.DiscreteInput)
                {
                    transactionCopy[remotePoint.Type].Add(remotePoint.Address, remotePoint);
                }
                else if (remotePoint.Type == RemotePointType.HoldingRegister || remotePoint.Type == RemotePointType.InputRegister)
                {
                    PopulateAnalogRemotePointAddresses(remotePoint);
                }
            }

            return true;
        }

        public abstract void Commit();

        public void Rollback()
        {
            transactionCopy = null;
        }

        public void RelaseTransactionResources()
        {
            transactionCopy = null;
        }

        private void PopulateAnalogRemotePointAddresses(RemotePoint remotePoint)
        {
            SortedList<ushort, RemotePoint> list = transactionCopy[remotePoint.Type];
            list.Add(remotePoint.Address, remotePoint);
            list.Add((ushort)(remotePoint.Address + 1), remotePoint);
        }

    }
}
