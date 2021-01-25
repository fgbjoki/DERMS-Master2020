using FieldProcessor.Model;
using FieldProcessor.TransactionProcessing.Storages;
using NUnit.Framework;
using System.Collections.Generic;

namespace UnitTests.SCADA.TransactionProcessing
{
    [TestFixture]
    public class TransactionStorageTests
    {
        [SetUp]
        public void SetUp()
        {

        }

        [Test]
        public void TransactionStorageTests_AnalogRemotePointStorage_ValidateSuccessfulTest()
        {
            // Assign
            long gid = 1;
            ushort address = 1;
            RemotePointType remotePointType = RemotePointType.HoldingRegister;

            RemotePoint remotePoint = new RemotePoint(gid, address, remotePointType);
            AnalogRemotePointStorage storage = new AnalogRemotePointStorage();

            // Act
            bool isValid = storage.ValidateEntity(remotePoint);

            // Assert
            Assert.True(isValid);
        }

        [Test]
        public void TransactionStorageTests_AnalogRemotePointStorage_ValidateFailedWrongTypeTest()
        {
            // Assign
            long gid = 1;
            ushort address = 1;
            RemotePointType remotePointType = RemotePointType.Coil;

            RemotePoint remotePoint = new RemotePoint(gid, address, remotePointType);
            AnalogRemotePointStorage storage = new AnalogRemotePointStorage();

            // Act
            bool isValid = storage.ValidateEntity(remotePoint);

            // Assert
            Assert.False(isValid);
        }

        [Test]
        public void TransactionStorageTests_AnalogRemotePointStorage_AddSuccessfulTest()
        {
            // Assign
            long gid = 1;
            ushort address = 1;
            RemotePointType remotePointType = RemotePointType.HoldingRegister;

            RemotePoint remotePoint = new RemotePoint(gid, address, remotePointType);
            AnalogRemotePointStorage storage = new AnalogRemotePointStorage();

            // Act
            bool isAdded = storage.AddEntity(remotePoint);
            bool entityExists = storage.EntityExists(gid);

            // Assert
            Assert.True(isAdded);
            Assert.True(entityExists);
        }

        [Test]
        public void TransactionStorageTests_AnalogRemotePointStorage_GetEntitySuccessfulTest()
        {
            // Assign
            long gid = 1;
            ushort address = 1;
            RemotePointType remotePointType = RemotePointType.HoldingRegister;

            RemotePoint remotePoint = new RemotePoint(gid, address, remotePointType);
            AnalogRemotePointStorage storage = new AnalogRemotePointStorage();

            // Act
            bool isAdded = storage.AddEntity(remotePoint);
            RemotePoint searchItem = storage.GetEntity(gid);

            // Assert
            Assert.True(isAdded);
            Assert.AreEqual(gid, searchItem.GlobalId);
        }

        [Test]
        public void TransactionStorageTests_AnalogRemotePointStorage_GetEntityFailedTest()
        {
            // Assign
            long gid = 1;
            ushort address = 1;
            RemotePointType remotePointType = RemotePointType.HoldingRegister;

            RemotePoint remotePoint = new RemotePoint(gid, address, remotePointType);
            AnalogRemotePointStorage storage = new AnalogRemotePointStorage();

            // Act
            bool isAdded = storage.AddEntity(remotePoint);
            RemotePoint searchItem = storage.GetEntity(gid + 1);

            // Assert
            Assert.True(isAdded);
            Assert.IsNull(searchItem);
        }

        [Test]
        public void TransactionStorageTests_AnalogRemotePointStorage_GetAllEntitiesTest()
        {
            // Assign
            long gid = 1;
            ushort address = 1;
            RemotePointType remotePointType = RemotePointType.HoldingRegister;

            RemotePoint remotePoint1 = new RemotePoint(gid, address, remotePointType);
            RemotePoint remotePoint2 = new RemotePoint(gid + 1, (ushort)(address + 1), remotePointType);

            List<RemotePoint> expectedList = new List<RemotePoint>() { remotePoint1, remotePoint2 };

            AnalogRemotePointStorage storage = new AnalogRemotePointStorage();

            // Act
            storage.AddEntity(remotePoint1);
            storage.AddEntity(remotePoint2);
            List<RemotePoint> entities = storage.GetAllEntities();

            // Assert
            CollectionAssert.AreEqual(expectedList, entities);
        }

        [Test]
        public void TransactionStorageTests_AnalogRemotePointStorage_AddAddressFailedAlreadyExsitsTest()
        {
            // Assign
            long gid = 1;
            ushort address = 1;
            RemotePointType remotePointType = RemotePointType.InputRegister;

            RemotePoint remotePoint1 = new RemotePoint(gid, address, remotePointType);
            RemotePoint remotePoint2 = new RemotePoint(gid + 1, address, remotePointType);
            AnalogRemotePointStorage storage = new AnalogRemotePointStorage();

            // Act
            bool isAdded = storage.AddEntity(remotePoint1);
            isAdded = storage.AddEntity(remotePoint2);

            // Assert
            Assert.False(isAdded);
        }

        [Test]
        public void TransactionStorageTests_AnalogRemotePointStorage_AddFailedNullTest()
        {
            // Assign
            AnalogRemotePointStorage storage = new AnalogRemotePointStorage();

            // Act
            bool isAdded = storage.AddEntity(null);

            // Assert
            Assert.False(isAdded);
        }

        [Test]
        public void TransactionStorageTests_DiscreteRemotePointStorage_ValidateSuccessfulTest()
        {
            // Assign
            long gid = 1;
            ushort address = 1;
            RemotePointType remotePointType = RemotePointType.Coil;

            RemotePoint remotePoint = new RemotePoint(gid, address, remotePointType);
            DiscreteRemotePointStorage storage = new DiscreteRemotePointStorage();

            // Act
            bool isValid = storage.ValidateEntity(remotePoint);

            // Assert
            Assert.True(isValid);
        }

        [Test]
        public void TransactionStorageTests_DiscreteRemotePointStorage_ValidateFailedWrongTypeTest()
        {
            // Assign
            long gid = 1;
            ushort address = 1;
            RemotePointType remotePointType = RemotePointType.HoldingRegister;

            RemotePoint remotePoint = new RemotePoint(gid, address, remotePointType);
            DiscreteRemotePointStorage storage = new DiscreteRemotePointStorage();

            // Act
            bool isValid = storage.ValidateEntity(remotePoint);

            // Assert
            Assert.False(isValid);
        }

        [Test]
        public void TransactionStorageTests_DiscreteRemotePointStorage_AddSuccessfulTest()
        {
            // Assign
            long gid = 1;
            ushort address = 1;
            RemotePointType remotePointType = RemotePointType.Coil;

            RemotePoint remotePoint = new RemotePoint(gid, address, remotePointType);
            DiscreteRemotePointStorage storage = new DiscreteRemotePointStorage();

            // Act
            bool isAdded = storage.AddEntity(remotePoint);
            bool entityExists = storage.EntityExists(gid);

            // Assert
            Assert.True(isAdded);
            Assert.True(entityExists);
        }

        [Test]
        public void TransactionStorageTests_DiscreteRemotePointStorage_GetAllEntitiesTest()
        {
            // Assign
            long gid = 1;
            ushort address = 1;
            RemotePointType remotePointType = RemotePointType.Coil;

            RemotePoint remotePoint1 = new RemotePoint(gid, address, remotePointType);
            RemotePoint remotePoint2 = new RemotePoint(gid + 1, (ushort)(address + 1), remotePointType);

            List<RemotePoint> expectedList = new List<RemotePoint>() { remotePoint1, remotePoint2 };

            DiscreteRemotePointStorage storage = new DiscreteRemotePointStorage();

            // Act
            storage.AddEntity(remotePoint1);
            storage.AddEntity(remotePoint2);
            List<RemotePoint> entities = storage.GetAllEntities();

            // Assert
            CollectionAssert.AreEqual(expectedList, entities);
        }

        [Test]
        public void TransactionStorageTests_DiscreteRemotePointStorage_GetEntitySuccessfulTest()
        {
            // Assign
            long gid = 1;
            ushort address = 1;
            RemotePointType remotePointType = RemotePointType.Coil;

            RemotePoint remotePoint = new RemotePoint(gid, address, remotePointType);
            DiscreteRemotePointStorage storage = new DiscreteRemotePointStorage();

            // Act
            bool isAdded = storage.AddEntity(remotePoint);
            RemotePoint searchItem = storage.GetEntity(gid);

            // Assert
            Assert.True(isAdded);
            Assert.AreEqual(gid, searchItem.GlobalId);
        }

        [Test]
        public void TransactionStorageTests_DiscreteRemotePointStorage_GetEntityFailedTest()
        {
            // Assign
            long gid = 1;
            ushort address = 1;
            RemotePointType remotePointType = RemotePointType.Coil;

            RemotePoint remotePoint = new RemotePoint(gid, address, remotePointType);
            DiscreteRemotePointStorage storage = new DiscreteRemotePointStorage();

            // Act
            bool isAdded = storage.AddEntity(remotePoint);
            RemotePoint searchItem = storage.GetEntity(gid + 1);

            // Assert
            Assert.True(isAdded);
            Assert.IsNull(searchItem);
        }

        [Test]
        public void TransactionStorageTests_DiscreteRemotePointStorage_AddAddressFailedAlreadyExsitsTest()
        {
            // Assign
            long gid = 1;
            ushort address = 1;
            RemotePointType remotePointType = RemotePointType.Coil;

            RemotePoint remotePoint1 = new RemotePoint(gid, address, remotePointType);
            RemotePoint remotePoint2 = new RemotePoint(gid + 1, address, remotePointType);
            DiscreteRemotePointStorage storage = new DiscreteRemotePointStorage();

            // Act
            bool isAdded = storage.AddEntity(remotePoint1);
            isAdded = storage.AddEntity(remotePoint2);

            // Assert
            Assert.False(isAdded);
        }

        [Test]
        public void TransactionStorageTests_DiscreteRemotePointStorage_AddFailedNullTest()
        {
            // Assign
            DiscreteRemotePointStorage storage = new DiscreteRemotePointStorage();

            // Act
            bool isAdded = storage.AddEntity(null);

            // Assert
            Assert.False(isAdded);
        }
    }
}
