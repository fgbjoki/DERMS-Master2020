using System.Collections.Generic;
using Common.ComponentStorage;
using Common.PubSub;
using Common.PubSub.Subscriptions;
using Common.AbstractModel;
using Common.ComponentStorage.StorageItemCreator;
using UIAdapter.TransactionProcessing.StorageItemCreators.DERGroup;
using UIAdapter.TransactionProcessing.StorageTransactionProcessors.DERGroup;
using System.Threading;
using Common.TransactionProcessing.Storage.Helpers;
using UIAdapter.PubSub.DynamicHandlers.DER;

namespace UIAdapter.TransactionProcessing.Storages.DERGroup
{
    public class DERGroupStorage : Storage<Model.DERGroup.DERGroup>, ISubscriber, IAnalogEntityStorage, IDERGroupStorage
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
            return new List<ISubscription>(2)
            {
                new Subscription(Topic.AnalogRemotePointChange, new EnergyStorageStateOfChargeDynamicHandler(this)),
                new Subscription(Topic.DERStateChange, new DERStateDynamicHandler(this))
            };
        }

        public void UpdateDERState(long derGid, float activePower)
        {
            locker.EnterReadLock();

            long derGroupGid = 0;
            if (generatorToDERGroupMapper.TryGetValue(derGid, out derGroupGid))
            {
                locker.ExitReadLock();

                var derGroup = GetEntity(derGroupGid);
                if (derGroup == null)
                {
                    return;
                }

                locker.EnterWriteLock();
                derGroup.Generator.ActivePower = activePower;
                locker.ExitWriteLock();
            }
            else
            {
                locker.ExitReadLock();

                var derGroup = GetEntity(derGid);
                if (derGroup == null)
                {
                    return;
                }

                locker.EnterWriteLock();
                derGroup.EnergyStorage.ActivePower = activePower;
                locker.ExitWriteLock();
            }          
        }

        public void UpdateAnalogValue(long measurementGid, float newValue)
        {
            long derGroupGid = 0;

            locker.EnterReadLock();

            AnalogValueOwner owner = analogEntityMapper.GetOwner(measurementGid);

            if (owner.OwnerGlobalId == 0)
            {
                locker.ExitReadLock();
                return;
            }

            if (!generatorToDERGroupMapper.TryGetValue(owner.OwnerGlobalId, out derGroupGid))
            {
                derGroupGid = owner.OwnerGlobalId;
            }

            locker.ExitReadLock();       

            var derGroup = GetEntity(derGroupGid);

            locker.EnterWriteLock();

            // TODO refactor this when implementing SETPOINT
            if (owner.MeasurementType == MeasurementType.Percent)
            {              
                derGroup.EnergyStorage.StateOfCharge = newValue;               
            }
            else if (owner.MeasurementType == MeasurementType.ActivePower)
            {
                if (derGroup.Generator.GlobalId == owner.OwnerGlobalId)
                {
                    derGroup.Generator.ActivePower = newValue;
                }
                else if (derGroup.EnergyStorage.GlobalId == owner.OwnerGlobalId)
                {
                    derGroup.EnergyStorage.ActivePower = newValue;
                }
            }

            locker.ExitWriteLock();
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
            addedMeasurement = analogEntityMapper.AddAnalogOwner(item.Generator.ActivePowerMeasurementGid, MeasurementType.ActivePower, item.EnergyStorage.GlobalId);
            addedMeasurement = analogEntityMapper.AddAnalogOwner(item.EnergyStorage.ActivePowerMeasurementGid, MeasurementType.ActivePower, item.EnergyStorage.GlobalId);
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
