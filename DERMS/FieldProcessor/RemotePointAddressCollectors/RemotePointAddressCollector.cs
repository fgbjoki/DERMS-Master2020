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
                transactionCopy[remotePoint.Type].Add(remotePoint.Address, remotePoint);
            }

            return true;
        }

        public abstract void Commit();

        //{
        //    locker.AcquireWriterLock(lockerTimeout);

        //    foreach (var pair in transactionCopy)
        //    {
        //        sortedAddressesPerRemotePointType[pair.Key] = new List<RemotePoint>(pair.Value.Values);
        //    }

        //    transactionCopy = null;

        //    locker.ReleaseWriterLock();
        //}

        public void Rollback()
        {
            transactionCopy = null;
        }

        public void RelaseTransactionResources()
        {
            transactionCopy = null;
        }
    }
}
