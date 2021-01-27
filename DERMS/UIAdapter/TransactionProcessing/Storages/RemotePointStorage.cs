using Common.AbstractModel;
using Common.ComponentStorage;
using Common.ComponentStorage.StorageItemCreator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIAdapter.Model;
using UIAdapter.TransactionProcessing.StorageItemCreators;
using UIAdapter.TransactionProcessing.StorageTransactionProcessors;

namespace UIAdapter.TransactionProcessing.Storages
{
    public class RemotePointStorage : Storage<RemotePoint>
    {
        public RemotePointStorage() : base("Remote Point Storage")
        {
        }

        public override List<IStorageTransactionProcessor> GetStorageTransactionProcessors()
        {
            Dictionary<DMSType, IStorageItemCreator> storageItemCreators = new Dictionary<DMSType, IStorageItemCreator>()
            {
                { DMSType.MEASUREMENTANALOG, new AnalogRemotePointStorageItemCreator() },
                { DMSType.MEASUREMENTDISCRETE, new DiscreteRemotePointStorageItemCreator() }
            };

            return new List<IStorageTransactionProcessor>()
            {
                new RemotePointTransactionProcessor(this, storageItemCreators)
            };
        }

        public override bool ValidateEntity(RemotePoint entity)
        {
            return true;
        }
    }
}
