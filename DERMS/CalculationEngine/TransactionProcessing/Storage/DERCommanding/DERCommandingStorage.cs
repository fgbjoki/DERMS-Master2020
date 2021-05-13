using CalculationEngine.Model.DERCommanding;
using CalculationEngine.TransactionProcessing.StorageItemCreators.DERCommanding;
using CalculationEngine.TransactionProcessing.StorageTransactionProcessor.DERCommanding;
using Common.AbstractModel;
using Common.ComponentStorage;
using Common.ComponentStorage.StorageItemCreator;
using Common.TransactionProcessing.Storage.Helpers;
using System.Collections.Generic;
using Common.PubSub;
using Common.PubSub.Subscriptions;
using CalculationEngine.PubSub.DynamicHandlers;

namespace CalculationEngine.TransactionProcessing.Storage.DERCommanding
{
    public class DERCommandingStorage : Storage<DistributedEnergyResource>, ISubscriber, IAnalogEntityStorage
    {
        private IAnalogEntityMapper analogEntityMapper;

        public DERCommandingStorage() : base("DER Commanding storage")
        {
            analogEntityMapper = new AnalogEntityMapper();
        }

        protected DERCommandingStorage(IAnalogEntityMapper analogEntityMapper) : base("DER Commanding storage")
        {
            this.analogEntityMapper = analogEntityMapper;
        }

        public override List<IStorageTransactionProcessor> GetStorageTransactionProcessors()
        {
            Dictionary<DMSType, IStorageItemCreator> creators = new Dictionary<DMSType, IStorageItemCreator>()
            {
                { DMSType.ENERGYSTORAGE, new EnergyStorageItemCreator() }
            };

            return new List<IStorageTransactionProcessor>() { new DERCommandingStorageTransactionProcessor(this, creators) };
        }

        public void UpdateAnalogValue(long measurementGid, float newValue)
        {
            locker.EnterReadLock();

            AnalogValueOwner analogOwner = analogEntityMapper.GetOwner(measurementGid);

            if (analogOwner.OwnerGlobalId == 0)
            {
                locker.ExitReadLock();
                return;
            }

            locker.ExitReadLock();

            var entity = GetEntity(analogOwner.OwnerGlobalId);

            locker.EnterWriteLock();

            // TODO refactor this when implementing SETPOINT
            if (analogOwner.MeasurementType == MeasurementType.Percent)
            {
                ((EnergyStorage)entity).StateOfCharge = newValue;
            }

            locker.ExitWriteLock();
        }
        
        public IEnumerable<ISubscription> GetSubscriptions()
        {
            return new List<ISubscription>()
            {
                new Subscription(Topic.AnalogRemotePointChange, new EnergyStorageStateOfChargeDynamicHandler(this))
            };
        }

        public override bool AddEntity(DistributedEnergyResource item)
        {
            bool entityAdded = base.AddEntity(item);

            DMSType entityDMSType = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(item.GlobalId);

            locker.EnterWriteLock();

            if (entityDMSType == DMSType.ENERGYSTORAGE)
            {
                entityAdded &= AddEnergyStorage(item as EnergyStorage);
            }

            locker.ExitWriteLock();

            return entityAdded;
        }

        protected override IStorage<DistributedEnergyResource> CreateNewStorage()
        {
            return new DERCommandingStorage(analogEntityMapper);
        }

        private bool AddEnergyStorage(EnergyStorage energyStorage)
        {
            bool addedMeasurement;
            addedMeasurement = analogEntityMapper.AddAnalogOwner(energyStorage.StateOfChargeMeasurementGid, MeasurementType.Percent, energyStorage.GlobalId);

            return addedMeasurement;
        }
    }
}
