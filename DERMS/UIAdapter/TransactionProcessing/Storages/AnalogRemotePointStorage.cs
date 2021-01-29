﻿using Common.AbstractModel;
using Common.ComponentStorage;
using Common.ComponentStorage.StorageItemCreator;
using System.Collections.Generic;
using UIAdapter.Model;
using UIAdapter.TransactionProcessing.StorageItemCreators;
using UIAdapter.TransactionProcessing.StorageTransactionProcessors;

namespace UIAdapter.TransactionProcessing.Storages
{
    public class AnalogRemotePointStorage : Storage<RemotePoint>
    {
        public AnalogRemotePointStorage() : base("Analog Remote Point Storage")
        {
        }

        public override List<IStorageTransactionProcessor> GetStorageTransactionProcessors()
        {
            Dictionary<DMSType, IStorageItemCreator> storageItemCreators = new Dictionary<DMSType, IStorageItemCreator>()
            {
                { DMSType.MEASUREMENTANALOG, new AnalogRemotePointStorageItemCreator() }
            };

            return new List<IStorageTransactionProcessor>()
            {
                new AnalogRemotePointTransactionProcessor(this, storageItemCreators)
            };
        }

        public override bool ValidateEntity(RemotePoint entity)
        {
            return true;
        }
    }
}