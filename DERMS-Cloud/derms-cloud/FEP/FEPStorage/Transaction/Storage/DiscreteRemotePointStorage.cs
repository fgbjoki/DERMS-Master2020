using Core.Common.AbstractModel;
using Core.Common.Transaction.Storage;
using Core.Common.Transaction.StorageItemCreator;
using Core.Common.Transaction.StorageTransactionProcessor;
using FEPStorage.Model;
using FEPStorage.Transaction.StorageTransactionProcessor;
using FieldProcessor.TransactionProcessing.StorageItemCreators;
using Microsoft.ServiceFabric.Data;
using System;
using System.Collections.Generic;

namespace FEPStorage.Transaction.Storage
{
    public class DiscreteRemotePointStorage : Storage<DiscreteRemotePoint>
    {
        private Action<string> logAction;

        public DiscreteRemotePointStorage(IReliableStateManager stateManager, Action<string> logAction) : base(stateManager, "Discrete remote point storage")
        {
            this.logAction = logAction;
        }

        protected DiscreteRemotePointStorage(IReliableStateManager stateManager, string storageName, string dictionaryName) : base(stateManager, storageName, dictionaryName)
        {

        }

        public override List<IStorageTransactionProcessor> GetStorageTransactionProcessors()
        {
            var storageItemCreators = new Dictionary<DMSType, IStorageItemCreator>()
            {
                { DMSType.MEASUREMENTDISCRETE, new DiscreteStorageItemCreator() }
            };

            //RemotePointSortedAddressCollector remotePointAddressCollector = ServiceLocator.GetService<RemotePointSortedAddressCollector>();
            //RemotePointRangeAddressCollector rangeAddressCollector = ServiceLocator.GetService<RemotePointRangeAddressCollector>();

            return new List<IStorageTransactionProcessor>() { new DiscreteTransactionProcessor(this, storageItemCreators, logAction) };
        }

        protected override IStorage<DiscreteRemotePoint> CreateNewStorage(string name, string dictionaryName)
        {
            return new DiscreteRemotePointStorage(StateManager, name, dictionaryName)
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
