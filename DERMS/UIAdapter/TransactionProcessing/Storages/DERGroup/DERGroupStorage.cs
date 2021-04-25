using System.Collections.Generic;
using Common.ComponentStorage;
using Common.PubSub;
using Common.PubSub.Subscriptions;
using Common.AbstractModel;
using Common.ComponentStorage.StorageItemCreator;
using UIAdapter.TransactionProcessing.StorageItemCreators.DERGroup;
using UIAdapter.TransactionProcessing.StorageTransactionProcessors.DERGroup;
using System.Threading;
using UIAdapter.TransactionProcessing.Storages.Helpers;
using UIAdapter.PubSub.DynamicHandlers.DERGroup;

namespace UIAdapter.TransactionProcessing.Storages.DERGroup
{
    public class DERGroupStorage : Storage<Model.DERGroup.DERGroup>, ISubscriber, IAnalogEntityStorage
    {
        private IAnalogEntityMapper analogEntityMapper;

        private long instanceCounter = 0;

        private Dictionary<long, long> generatorToDERGroupMapper;

        public DERGroupStorage() : base("DERGroup storage")
        {
            generatorToDERGroupMapper = new Dictionary<long, long>();
            analogEntityMapper = new AnalogEntityMapper();
        }

        protected DERGroupStorage(IAnalogEntityMapper analogEntityMapper, Dictionary<long, long> generatorToDERGroupMapper) : base("DERGroup storage")
        {
            this.analogEntityMapper = analogEntityMapper;
            this.generatorToDERGroupMapper = generatorToDERGroupMapper;
        }

        public override List<IStorageTransactionProcessor> GetStorageTransactionProcessors()
        {
            Dictionary<DMSType, IStorageItemCreator> itemCreators = new Dictionary<DMSType, IStorageItemCreator>()
            {
                { DMSType.ENERGYSTORAGE, new DERGroupStorageItemCreator() }
            };

            return new List<IStorageTransactionProcessor>(1) { new DERGroupStorageTransactionProcessor(this, itemCreators) };
        }

        public IEnumerable<ISubscription> GetSubscriptions()
        {
            return new List<ISubscription>(1)
            {
                new Subscription(Topic.AnalogRemotePointChange, new EnergyStorageStateOfChargeDynamicHandler(this))
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

            var derGroup = GetEntity(owner.OwnerGlobalId);

            if (owner.MeasurementType == MeasurementType.Percent)
            {
                locker.EnterWriteLock();
                derGroup.EnergyStorage.StateOfCharge = newValue;
                locker.ExitWriteLock();
            }
        }

        public override bool AddEntity(Model.DERGroup.DERGroup item)
        {
            if (!base.AddEntity(item))
            {
                return false;
            }

            Interlocked.Increment(ref instanceCounter);

            bool addedMeasurement;

            locker.EnterWriteLock();

            item.Name = $"DERGroup {instanceCounter}";

            addedMeasurement = analogEntityMapper.AddAnalogOwner(item.EnergyStorage.StateOfChargeMeasurementGid, MeasurementType.Percent, item.EnergyStorage.GlobalId);

            if (item.Generator != null)
            {
                generatorToDERGroupMapper.Add(item.Generator.GlobalId, item.GlobalId);
            }

            locker.ExitWriteLock();

            return addedMeasurement;
        }

        protected override IStorage<Model.DERGroup.DERGroup> CreateNewStorage()
        {
            return new DERGroupStorage(analogEntityMapper, generatorToDERGroupMapper);
        }
    }
}
