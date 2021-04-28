using CalculationEngine.Model.DERStates;
using CalculationEngine.TransactionProcessing.StorageItemCreators.DERstates;
using CalculationEngine.TransactionProcessing.StorageTransactionProcessor.DERState;
using Common.AbstractModel;
using Common.ComponentStorage;
using Common.ComponentStorage.StorageItemCreator;
using Common.TransactionProcessing.Storage.Helpers;
using System.Collections.Generic;

namespace CalculationEngine.TransactionProcessing.Storage.DERStates
{
    public class DERStateStorage : Storage<DERState>, IAnalogEntityStorage
    {
        private IAnalogEntityMapper analogEntityMapper;

        public DERStateStorage() : base("DERState storage")
        {
            analogEntityMapper = new AnalogEntityMapper();
        }

        protected DERStateStorage(IAnalogEntityMapper analogEntityMapper) : this()
        {
            this.analogEntityMapper = analogEntityMapper;
        }

        public override List<IStorageTransactionProcessor> GetStorageTransactionProcessors()
        {
            IStorageItemCreator derStorageItemCreator = new DERStateStorageItemCreator();
            Dictionary<DMSType, IStorageItemCreator> storageItemCreators = new Dictionary<DMSType, IStorageItemCreator>()
            {
                { DMSType.WINDGENERATOR, derStorageItemCreator },
                { DMSType.SOLARGENERATOR, derStorageItemCreator },
                { DMSType.ENERGYSTORAGE, derStorageItemCreator },
            };

            return new List<IStorageTransactionProcessor>(1)
            {
                new DERStateStorageTransactionProcessor(this, storageItemCreators)
            };
        }

        public void UpdateAnalogValue(long measurementGid, float newValue)
        {
            locker.EnterReadLock();

            AnalogValueOwner owner = analogEntityMapper.GetOwner(measurementGid);

            if (owner.OwnerGlobalId == 0)
            {
                locker.ExitReadLock();
                return;
            }

            locker.ExitReadLock();

            var derState = GetEntity(owner.OwnerGlobalId);

            if (owner.MeasurementType == MeasurementType.ActivePower)
            {
                locker.EnterWriteLock();
                derState.ActivePower = newValue;
                locker.ExitWriteLock();
            }
        }

        public override bool AddEntity(DERState item)
        {
            if (!base.AddEntity(item))
            {
                return false;
            }

            bool addedMeasurement;

            locker.EnterWriteLock();

            addedMeasurement = analogEntityMapper.AddAnalogOwner(item.ActivePowerMeasurementGid, MeasurementType.ActivePower, item.GlobalId);

            locker.ExitWriteLock();

            return addedMeasurement;
        }

        public DERState GetEntityByAnalogMeasurementGid(long analogMeasurementGid)
        {
            locker.EnterReadLock();

            AnalogValueOwner owner = analogEntityMapper.GetOwner(analogMeasurementGid);

            if (owner.OwnerGlobalId == 0)
            {
                locker.ExitReadLock();
                return null;
            }

            locker.ExitReadLock();

            return GetEntity(owner.OwnerGlobalId);
        }

        protected override IStorage<DERState> CreateNewStorage()
        {
            return new DERStateStorage(analogEntityMapper);
        }
    }
}
