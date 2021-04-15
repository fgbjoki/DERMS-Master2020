using System;
using System.Collections.Generic;
using FieldSimulator.Model;
using System.Threading;

namespace FieldSimulator.PowerSimulator.Storage
{
    abstract class RemotePointStorage<ValueType> : IRemotePointStorageStorage<ValueType>
    {
        private Dictionary<int, ValueType> values;

        private ReaderWriterLockSlim locker;

        public RemotePointStorage()
        {
            values = new Dictionary<int, ValueType>();
            locker = new ReaderWriterLockSlim();
        }

        public bool AddItem(RemotePointType remotePointType, ushort address, ValueType value)
        {
            int key = GetRemotePointHashCode(remotePointType, address);

            if (EntityExists(remotePointType, address))
            {
                return false;
            }

            locker.EnterWriteLock();

            values.Add(key, value);

            locker.ExitWriteLock();

            return true;
        }

        public ValueType GetValue(RemotePointType remotePointType, ushort address)
        {
            int key = GetRemotePointHashCode(remotePointType, address);

            if (!EntityExists(remotePointType, address))
            {
                throw new ArgumentException($"Cannot find remote point with type {remotePointType} and addres {address}.");
            }

            locker.EnterReadLock();

            ValueType value = values[key];

            locker.ExitReadLock();

            return value;
        }

        public bool UpdateValue(RemotePointType remotePointType, ushort address, ValueType newValue)
        {
            int key = GetRemotePointHashCode(remotePointType, address);

            if (EntityExists(remotePointType, address))
            {
                return false;
            }

            locker.EnterWriteLock();

            values[key] = newValue;

            locker.ExitWriteLock();

            return true;
        }

        private int GetRemotePointHashCode(RemotePointType remotePointType, ushort address)
        {
            return (ushort)remotePointType | (address << sizeof(ushort) * 8);
        }

        public bool EntityExists(RemotePointType remotePointType, ushort address)
        {
            int key = GetRemotePointHashCode(remotePointType, address);
            bool entityExists;

            locker.EnterReadLock();

            entityExists = values.ContainsKey(key);

            locker.ExitReadLock();

            return entityExists;
        }
    }
}
