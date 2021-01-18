using Common.AbstractModel;
using Common.ComponentStorage;
using System.Collections.Generic;
using Common.ComponentStorage.StorageItemCreator;

namespace UnitTests.Common.TransactionProcessing.TestClasses
{
    class StorageTransactionProcessorTestClass : StorageTransactionProcessor<TestObject>
    {
        public StorageTransactionProcessorTestClass(IStorage<TestObject> storage, Dictionary<DMSType, IStorageItemCreator> storageItemCreators) : base(storage, storageItemCreators)
        {
        }

        protected override List<DMSType> GetPrimaryTypes()
        {
            return new List<DMSType>() { DMSType.ENERGYCONSUMER };
        }
    }
}
