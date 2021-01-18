using Common.ComponentStorage;
using System.Collections.Generic;
using Common.AbstractModel;
using Common.ComponentStorage.StorageItemCreator;
using FieldProcessor.Model;

namespace FieldProcessor.TransactionProcessing
{
    public class AnalogTransactionProcessor : StorageTransactionProcessor<RemotePoint>
    {
        public AnalogTransactionProcessor(IStorage<RemotePoint> storage, Dictionary<DMSType, IStorageItemCreator> storageItemCreators) : base(storage, storageItemCreators)
        {
        }

        protected override List<DMSType> GetPrimaryTypes()
        {
            return new List<DMSType>() { DMSType.MEASUREMENTANALOG };
        }
    }
}
