using Common.SCADA;
using Common.ComponentStorage;
using NetworkDynamicsService.Model.RemotePoints;
using NServiceBus;
using Common.GDA;
using System.Threading;
using Common.Logger;
using System.Collections.Generic;
using Common.PubSub.Messages;
using System;

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
            BaseMessageEntitiesChanged<ResourceDescription> publicationChanges = null;

            foreach (var fieldValue in fieldValues)
            {
                locker.EnterReadLock();
                RemotePointType remotePoint = storage.GetEntity(fieldValue.GlobalId);
                locker.ExitReadLock();

                if (remotePoint == null)
                {
                    Logger.Instance.Log($"[{GetType().Name}] Coudln't find remote point to process with gid: 0x{fieldValue.GlobalId:X16}.");
                    continue;
                }

                if (!HasValueChanged(remotePoint, fieldValue.Value))
                {
                    continue;
                }

                locker.EnterWriteLock();
                ResourceDescription changes = ApplyChanges(remotePoint, fieldValue.Value);
                locker.ExitWriteLock();

                // TODO UNCOMMENT THIS WHEN IMPLEMENTING DATABASE MANIPULATION
                //SaveChanges(remotePoint);

                AddChangeToPublication(ref publicationChanges, changes);
            }

            return publicationChanges;         
        }

        /// <summary>
        /// Applies all needed changes on <paramref name="remotePoint"/>.
        /// </summary>
        /// <returns><b>True</b> if the remote point was changes, otherwise <b>false</b>.</returns>
        protected abstract bool HasValueChanged(RemotePointType remotePoint, int rawValue);

        protected abstract ResourceDescription ApplyChanges(RemotePointType remotePoint, int rawValue);

        protected abstract BaseMessageEntitiesChanged<ResourceDescription> CreatePublication();

        // TODO UNCOMMENT THIS WHEN IMPLEMENTING DATABASE MANIPULATION
        //protected abstract void SaveChanges(RemotePointType remotePoint);

        private void AddChangeToPublication(ref BaseMessageEntitiesChanged<ResourceDescription> publicationChanges, ResourceDescription changes)
        {
            if (publicationChanges == null)
            {
                publicationChanges = CreatePublication();
            }

            if (changes != null)
            {
                publicationChanges.Changes.Add(changes);
            }
        }
    }
}
