using Core.Common.AbstractModel;
using Core.Common.ReliableCollectionProxy;
using Core.Common.Transaction.Models.FEP.FEPStorage;
using Core.Common.Transaction.Storage;
using Core.Common.Transaction.StorageItemCreator;
using Core.Common.Transaction.StorageTransactionProcessor;
using FEPStorage.Model;
using FieldProcessor.TransactionProcessing;
using FieldProcessor.TransactionProcessing.StorageItemCreators;
using Microsoft.ServiceFabric.Data;
using System;
using System.Collections.Generic;

namespace FEPStorage.Transaction.Storage
{
    public class AnalogRemotePointStorage : Storage<AnalogRemotePoint>
    {
        private string usedAddressesDictionary;

        private Action<string> logAction;

        public AnalogRemotePointStorage(IReliableStateManager stateManager, Action<string> logAction) : base(stateManager, "Analog remote point storage")
        {
            this.logAction = logAction;

            usedAddressesDictionary = "usedAddresses" + storageName;
            ReliableDictionaryProxy.CreateDictionary<HashSet<ushort>, int>(stateManager, usedAddressesDictionary);
        }

        protected AnalogRemotePointStorage(IReliableStateManager stateManager, string storageName, string dictionaryName) : base(stateManager, storageName, dictionaryName)
        {
            usedAddressesDictionary = "usedAddresses" + storageName;
            ReliableDictionaryProxy.CreateDictionary<HashSet<ushort>, int>(stateManager, usedAddressesDictionary);
        }

        public override List<IStorageTransactionProcessor> GetStorageTransactionProcessors()
        {
            var storageItemCreators = new Dictionary<DMSType, IStorageItemCreator>()
            {
                { DMSType.MEASUREMENTANALOG, new AnalogStorageItemCreator() }
            };

            //RemotePointSortedAddressCollector remotePointAddressCollector = ServiceLocator.GetService<RemotePointSortedAddressCollector>();
            //RemotePointRangeAddressCollector remoteRangeAddressCollector = ServiceLocator.GetService<RemotePointRangeAddressCollector>();

            return new List<IStorageTransactionProcessor>() { new AnalogTransactionProcessor(this, storageItemCreators, logAction) };
        }

        public override bool AddEntity(AnalogRemotePoint entity)
        {
            if (entity == null)
            {
                Log($"[{storageName}] Cannot add null to storage!");
                return false;
            }

            HashSet<ushort> addresses = ReliableDictionaryProxy.GetEntity<HashSet<ushort>, int>(StateManager, (int)entity.Type, usedAddressesDictionary);
            if (addresses == null)
            {
                addresses = new HashSet<ushort>();
                ReliableDictionaryProxy.AddOrUpdateEntity(stateManager, addresses, (int)entity.Type, usedAddressesDictionary);
            }

            if (addresses.Contains(entity.Address))
            {
                Log($"[{storageName}] Remote point with address : {entity.Address} already exists!");
                return false;
            }

            addresses.Add(entity.Address);
            ReliableDictionaryProxy.AddOrUpdateEntity(stateManager, addresses, (int)entity.Type, usedAddressesDictionary);

            bool addSucceded = base.AddEntity(entity);

            if (!addSucceded)
            {
                addresses.Remove(entity.Address);
                ReliableDictionaryProxy.AddOrUpdateEntity(stateManager, addresses, (int)entity.Type, usedAddressesDictionary);
            }

            return addSucceded;
        }

        public override bool ValidateEntity(AnalogRemotePoint entity)
        {
            return base.ValidateEntity(entity) &&
                (entity.Type == RemotePointType.HoldingRegister || entity.Type == RemotePointType.InputRegister);
        }

        public override void ShallowCopyEntities(IStorage<AnalogRemotePoint> storage)
        {
            base.ShallowCopyEntities(storage);

            AnalogRemotePointStorage analogRemotePointStorage = storage as AnalogRemotePointStorage;

            ReliableDictionaryProxy.CopyDictionary<HashSet<ushort>, int>(StateManager, usedAddressesDictionary, analogRemotePointStorage.usedAddressesDictionary);
        }

        protected override IStorage<AnalogRemotePoint> CreateNewStorage(string name, string dictionaryName)
        {
            return new AnalogRemotePointStorage(StateManager, name, dictionaryName)
            {
                logAction = logAction,
                usedAddressesDictionary = usedAddressesDictionary + "cloned"
            };
        }

        protected override void Log(string text)
        {
            logAction(text);
        }
    }
}
