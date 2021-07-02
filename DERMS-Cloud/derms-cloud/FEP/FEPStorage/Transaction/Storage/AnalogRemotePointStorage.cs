using Core.Common.AbstractModel;
using Core.Common.ReliableCollectionProxy;
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
        private readonly string usedAddressesDictionary;

        private Action<string> logAction;

        public AnalogRemotePointStorage(IReliableStateManager stateManager, Action<string> logAction) : base(stateManager, "Analog remote point storage")
        {
            this.logAction = logAction;

            usedAddressesDictionary = "usedAddresses" + storageName;
            ReliableDictionaryProxy.CreateDictionary<HashSet<ushort>, RemotePointTypeWrapper>(stateManager, usedAddressesDictionary);
        }

        protected AnalogRemotePointStorage(IReliableStateManager stateManager, string storageName, string dictionaryName) : base(stateManager, storageName, dictionaryName)
        {
            usedAddressesDictionary = "usedAddresses" + storageName;
            ReliableDictionaryProxy.CreateDictionary<HashSet<ushort>, RemotePointTypeWrapper>(stateManager, usedAddressesDictionary);
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

        protected override IStorage<AnalogRemotePoint> CreateNewStorage(string name, string dictionaryName)
        {
            return new AnalogRemotePointStorage(StateManager, name, dictionaryName)
            {
                logAction = logAction
            };
        }

        protected override void Log(string text)
        {
            logAction(text);
        }
    }
}
