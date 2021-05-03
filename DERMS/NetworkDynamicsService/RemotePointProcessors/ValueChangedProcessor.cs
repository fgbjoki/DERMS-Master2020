using Common.SCADA;
using Common.ComponentStorage;
using NetworkDynamicsService.Model.RemotePoints;
using NServiceBus;
using Common.GDA;
using System.Threading;
using Common.Logger;
using System.Collections.Generic;

namespace NetworkDynamicsService.RemotePointProcessors
{
    public abstract class ValueChangedProcessor<RemotePointType> : IValueChangedProcessor
        where RemotePointType : RemotePoint
    {
        private ReaderWriterLockSlim locker;
        private IStorage<RemotePointType> storage;

        public ValueChangedProcessor(IStorage<RemotePointType> storage)
        {
            this.storage = storage;

            locker = new ReaderWriterLockSlim();
        }

        public IEvent ProcessChangedValue(IEnumerable<RemotePointFieldValue> fieldValues)
        {
            List<ResourceDescription> publicationChanges = CreatePublication();

            foreach (var fieldValue in fieldValues)
            {
                locker.EnterReadLock();
                RemotePointType remotePoint = storage.GetEntity(fieldValue.GlobalId);
                locker.ExitReadLock();

                if (remotePoint == null)
                {
                    Logger.Instance.Log($"[{GetType()}] Coudln't find remote point to process with gid: 0x{fieldValue.GlobalId:X16}.");
                    return null;
                }

                if (!HasValueChanged(remotePoint, fieldValue.Value))
                {
                    return null;
                }

                locker.EnterWriteLock();
                ResourceDescription changes = ApplyChanges(remotePoint, fieldValue.Value);
                locker.ExitWriteLock();

                // TODO UNCOMMENT THIS WHEN IMPLEMENTING DATABASE MANIPULATION
                //SaveChanges(remotePoint);

                if (changes != null)
                {
                    publicationChanges.Add(changes);
                }
            }

            return GetPublication(publicationChanges);          
        }

        protected abstract IEvent GetPublication(List<ResourceDescription> publicationChanges);

        /// <summary>
        /// Applies all needed changes on <paramref name="remotePoint"/>.
        /// </summary>
        /// <returns><b>True</b> if the remote point was changes, otherwise <b>false</b>.</returns>
        protected abstract bool HasValueChanged(RemotePointType remotePoint, int rawValue);

        protected abstract ResourceDescription ApplyChanges(RemotePointType remotePoint, int rawValue);

        protected abstract List<ResourceDescription> CreatePublication();

        // TODO UNCOMMENT THIS WHEN IMPLEMENTING DATABASE MANIPULATION
        //protected abstract void SaveChanges(RemotePointType remotePoint);
    }
}
