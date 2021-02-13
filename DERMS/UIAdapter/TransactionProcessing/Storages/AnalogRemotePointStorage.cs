using Common.AbstractModel;
using Common.ComponentStorage;
using Common.ComponentStorage.StorageItemCreator;
using System.Collections.Generic;
using UIAdapter.Model;
using UIAdapter.TransactionProcessing.StorageItemCreators;
using UIAdapter.TransactionProcessing.StorageTransactionProcessors;
using NServiceBus;
using System;
using UIAdapter.DynamicHandlers;

namespace UIAdapter.TransactionProcessing.Storages
{
    public class AnalogRemotePointStorage : Storage<AnalogRemotePoint>, INServiceBusStorage
    {
        public AnalogRemotePointStorage() : base("Analog Remote Point Storage")
        {
        }

        public List<object> GetHandlers()
        {
            return new List<object>() { new AnalogRemotePointChangedHandler(this) };
        }

        public override List<IStorageTransactionProcessor> GetStorageTransactionProcessors()
        {
            Dictionary<DMSType, IStorageItemCreator> storageItemCreators = new Dictionary<DMSType, IStorageItemCreator>()
            {
                { DMSType.MEASUREMENTANALOG, new AnalogRemotePointStorageItemCreator() }
            };

            return new List<IStorageTransactionProcessor>()
            {
                new AnalogRemotePointTransactionProcessor(this, storageItemCreators, commitDone)
            };
        }

        public override bool ValidateEntity(AnalogRemotePoint entity)
        {
            return true;
        }
    }
}
