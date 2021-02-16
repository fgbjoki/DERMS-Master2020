using Common.GDA;
using Common.AbstractModel;
using Common.ComponentStorage;
using Common.ComponentStorage.StorageItemCreator;
using System.Collections.Generic;

namespace UnitTests.Common.TransactionProcessing.TestClasses
{
    public class StorageItemCreatorTest : StorageItemCreator
    {
        public StorageItemCreatorTest(Dictionary<DMSType, List<ModelCode>> propertiesPerType) : base(propertiesPerType)
        {
        }

        public override IdentifiedObject CreateStorageItem(ResourceDescription rd, Dictionary<DMSType, List<ResourceDescription>> affectedEntities)
        {
            return new TestObject(rd.Id);
        }
    }
}
