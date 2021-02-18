using Common.ComponentStorage;
using NUnit.Framework;
using System.Collections.Generic;
using Common.AbstractModel;
using NSubstitute;
using Common.ComponentStorage.StorageItemCreator;
using Common.GDA;
using UnitTests.Common.TransactionProcessing.TestClasses;

namespace UnitTests.Common.TransactionProcessing
{
    [TestFixture]
    public class StorageTransactionProcessorTests
    {
        IStorage<TestObject> storage;

        [SetUp]
        public void SetUp()
        {
            storage = Substitute.For<IStorage<TestObject>>();
        }

        [Test]
        public void StorageTransactionProcessor_ApplyChanges_SuccessfulTest()
        {
            // Assign
            DMSType dmsType = DMSType.ENERGYCONSUMER;
            List<long> newEntities = CreateGids(dmsType, 5);

            Dictionary<DMSType, HashSet<long>> neededEntities = new Dictionary<DMSType, HashSet<long>>();
            Dictionary<DMSType, List<long>> insertedEntities = new Dictionary<DMSType, List<long>>()
            {
                { dmsType, newEntities },
                { DMSType.CONNECTIVITYNODE, new List<long>() { 1 } }
            };

            StorageTransactionProcessorTestClass testingClass = new StorageTransactionProcessorTestClass(storage, new Dictionary<DMSType, IStorageItemCreator>());

            // Act
            bool applyChangesSucceded = testingClass.ApplyChanges(insertedEntities, neededEntities);

            // Assert
            Assert.True(applyChangesSucceded);
            CollectionAssert.AreEqual(newEntities, neededEntities[dmsType]);
        }

        [Test]
        public void StorageTransactionProcessor_ApplyChanges_FailedEntityExsitsTest()
        {
            // Assign
            ModelCode modelCodeType = ModelCode.ENERGYCONSUMER;
            DMSType dmsType = DMSType.ENERGYCONSUMER;
            List<long> newEntities = CreateGids(dmsType, 5);

            Dictionary<DMSType, HashSet<long>> neededEntities = new Dictionary<DMSType, HashSet<long>>();
            Dictionary<DMSType, List<long>> insertedEntities = new Dictionary<DMSType, List<long>>()
            {
                { dmsType, newEntities },
                { DMSType.CONNECTIVITYNODE, new List<long>() { 1 } }
            };

            Dictionary<DMSType, List<ModelCode>> itemCreatorProperties = new Dictionary<DMSType, List<ModelCode>>()
            {
                { dmsType, new List<ModelCode>() { ModelCode.ENERGYCONSUMER_PFIXED } }
            };

            StorageItemCreatorTest itemCreator = new StorageItemCreatorTest(itemCreatorProperties);
            StorageTransactionProcessorTestClass testingClass = new StorageTransactionProcessorTestClass(storage, new Dictionary<DMSType, IStorageItemCreator>() { { dmsType, itemCreator } });

            storage.EntityExists(Arg.Compat.Any<long>()).Returns(true);

            bool succeded = testingClass.ApplyChanges(insertedEntities, neededEntities);

            // Assert
            Assert.False(succeded);
        }

        [Test]
        public void StorageTransactionProcessor_Prepare_SuccededTest()
        {
            // Assign
            ModelCode modelCodeType = ModelCode.ENERGYCONSUMER;
            DMSType dmsType = ModelCodeHelper.GetTypeFromModelCode(modelCodeType);

            List<long> newEntities = CreateGids(dmsType, 5);

            Dictionary<DMSType, List<ResourceDescription>> affectedEntities = new Dictionary<DMSType, List<ResourceDescription>>();
            affectedEntities[dmsType] = new List<ResourceDescription>();

            Dictionary<DMSType, List<ModelCode>> itemCreatorProperties = new Dictionary<DMSType, List<ModelCode>>()
            {
                { dmsType, new List<ModelCode>() { ModelCode.ENERGYCONSUMER_PFIXED } }
            };

            StorageItemCreatorTest itemCreator = Substitute.For<StorageItemCreatorTest>(itemCreatorProperties);

            foreach (var newEntity in newEntities)
            {
                ResourceDescription rd = new ResourceDescription(newEntity);
                affectedEntities[dmsType].Add(rd);
                itemCreator.CreateStorageItem(rd, affectedEntities).Returns(new TestObject(rd.Id));
            }

            storage.ValidateEntity(Arg.Compat.Any<TestObject>()).Returns(true);
            storage.Clone().Returns(storage);

            StorageTransactionProcessorTestClass testingClass = new StorageTransactionProcessorTestClass(storage, new Dictionary<DMSType, IStorageItemCreator>() { { dmsType, itemCreator } });

            // Act
            bool isSuccessful = testingClass.Prepare(affectedEntities);

            // Assert
            Assert.True(isSuccessful);
            itemCreator.Received(newEntities.Count).CreateStorageItem(Arg.Compat.Any<ResourceDescription>(), affectedEntities);
        }

        [Test]
        public void StorageTransactionProcessor_Prepare_FailedValidationTest()
        {
            // Assign
            ModelCode modelCodeType = ModelCode.ENERGYCONSUMER;
            DMSType dmsType = ModelCodeHelper.GetTypeFromModelCode(modelCodeType);

            List<long> newEntities = CreateGids(dmsType, 5);

            Dictionary<DMSType, List<ResourceDescription>> affectedEntities = new Dictionary<DMSType, List<ResourceDescription>>();
            affectedEntities[dmsType] = new List<ResourceDescription>();

            Dictionary<DMSType, List<ModelCode>> itemCreatorProperties = new Dictionary<DMSType, List<ModelCode>>()
            {
                { dmsType, new List<ModelCode>() { ModelCode.ENERGYCONSUMER_PFIXED } }
            };

            StorageItemCreatorTest itemCreator = Substitute.For<StorageItemCreatorTest>(itemCreatorProperties);

            foreach (var newEntity in newEntities)
            {
                ResourceDescription rd = new ResourceDescription(newEntity);
                affectedEntities[dmsType].Add(rd);
                itemCreator.CreateStorageItem(rd, affectedEntities).Returns(new TestObject(rd.Id));
            }

            storage.ValidateEntity(Arg.Compat.Any<TestObject>()).Returns(false);
            storage.Clone().Returns(storage);

            StorageTransactionProcessorTestClass testingClass = new StorageTransactionProcessorTestClass(storage, new Dictionary<DMSType, IStorageItemCreator>() { { dmsType, itemCreator } });

            // Act
            bool isSuccessful = testingClass.Prepare(affectedEntities);

            // Assert
            Assert.False(isSuccessful);
        }

        [Test]
        public void StorageTransactionProcessor_GetNeededPropertiesTest()
        {
            // Assign
            ModelCode modelCodeType = ModelCode.ENERGYCONSUMER;
            DMSType dmsType = ModelCodeHelper.GetTypeFromModelCode(modelCodeType);

            List<long> newEntities = CreateGids(dmsType, 5);

            Dictionary<DMSType, List<ResourceDescription>> affectedEntities = new Dictionary<DMSType, List<ResourceDescription>>();
            affectedEntities[dmsType] = new List<ResourceDescription>();

            Dictionary<DMSType, List<ModelCode>> itemCreatorProperties1 = new Dictionary<DMSType, List<ModelCode>>()
            {
                { dmsType, new List<ModelCode>() { ModelCode.ENERGYCONSUMER_PFIXED } },
                { DMSType.SOLARGENERATOR, new List<ModelCode>() { ModelCode.DER_ACTIVEPOWER } }
            };

            Dictionary<DMSType, List<ModelCode>> itemCreatorProperties2 = new Dictionary<DMSType, List<ModelCode>>()
            {
                { DMSType.SOLARGENERATOR, new List<ModelCode>() { ModelCode.DER_NOMINALPOWER } }
            };

            StorageItemCreatorTest itemCreator1 = new StorageItemCreatorTest(itemCreatorProperties1);
            StorageItemCreatorTest itemCreator2 = new StorageItemCreatorTest(itemCreatorProperties2); 

            StorageTransactionProcessorTestClass testingClass = new StorageTransactionProcessorTestClass(storage, new Dictionary<DMSType, IStorageItemCreator>() { { dmsType, itemCreator1 }, { DMSType.SOLARGENERATOR, itemCreator2 } });

            // Act
            Dictionary<DMSType, List<ModelCode>> properties = testingClass.GetNeededProperties();

            // Assert
            Assert.True(properties[DMSType.ENERGYCONSUMER].Contains(ModelCode.ENERGYCONSUMER_PFIXED));
            Assert.True(properties[DMSType.SOLARGENERATOR].Contains(ModelCode.DER_ACTIVEPOWER));
            Assert.True(properties[DMSType.SOLARGENERATOR].Contains(ModelCode.DER_NOMINALPOWER));
        }

        private List<long> CreateGids(DMSType dmsType, int count)
        {
            List<long> newGids = new List<long>();

            for (int i = 0; i < count; i++)
            {
                newGids.Add(ModelCodeHelper.CreateGlobalId(0, (short)dmsType, i));
            }

            return newGids;
        }
    }
}
