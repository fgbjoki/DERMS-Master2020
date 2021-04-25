using Common.GDA;
using Common.AbstractModel;
using Common.ComponentStorage;
using Common.ComponentStorage.StorageItemCreator;
using System.Collections.Generic;
using System;

namespace UnitTests.Common.TransactionProcessing.TestClasses
{
    public class StorageItemCreatorTest : StorageItemCreator
    {
        private Dictionary<DMSType, List<ModelCode>> propertiesPerType;
        public StorageItemCreatorTest(Dictionary<DMSType, List<ModelCode>> propertiesPerType) : base()
        {
            this.propertiesPerType = propertiesPerType;
        }

        public override IdentifiedObject CreateStorageItem(ResourceDescription rd, Dictionary<DMSType, List<ResourceDescription>> affectedEntities)
        {
            return new TestObject(rd.Id);
        }

        public override Dictionary<DMSType, List<ModelCode>> GetNeededProperties()
        {
            return propertiesPerType;
        }
    }
}
