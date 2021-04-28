using CalculationEngine.Model.EnergyCalculations;
using Common.ComponentStorage;
using System.Collections.Generic;
using System.Threading;
using Common.AbstractModel;
using Common.Logger;
using Common.ComponentStorage.StorageItemCreator;
using CalculationEngine.TransactionProcessing.StorageItemCreators.EnergyBalance;
using CalculationEngine.TransactionProcessing.StorageTransactionProcessor.EnergyBalance;
using System;

namespace CalculationEngine.TransactionProcessing.Storage.EnergyBalance
{
    public class EnergyBalanceStorage : IStorage<CalculationObject>, ITransactionStorage
    {
        private Dictionary<long, long> analogMeasurementToEntityMap;
        private ReaderWriterLockSlim locker;

        public EnergyBalanceStorage()
        {
            EnergyConsumerStorage = new EnergyConsumerStorage();
            EnergyGeneratorStorage = new EnergyGeneratorStorage();
            EnergySourceStorage = new EnergySourceStorage();

            analogMeasurementToEntityMap = new Dictionary<long, long>();
            locker = new ReaderWriterLockSlim();
        }

        public AutoResetEvent Commited
        {
            get
            {
                return EnergySourceStorage.Commited;
            }
        }

        public bool AddEntity(CalculationObject entity)
        {
            bool added = false;

            DMSType entityType = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(entity.GlobalId);
            switch (entityType)
            {
                case DMSType.ENERGYCONSUMER:
                    added = EnergyConsumerStorage.AddEntity(entity as EnergyConsumer);
                    break;
                case DMSType.ENERGYSTORAGE:
                case DMSType.SOLARGENERATOR:
                case DMSType.WINDGENERATOR:
                    added = EnergyGeneratorStorage.AddEntity(entity as EnergyGenerator);
                    break;
                case DMSType.ENERGYSOURCE:
                    added = EnergySourceStorage.AddEntity(entity as EnergySource);
                    break;
            }

            if (added)
            {
                locker.EnterWriteLock();

                long analongMeasurementGid = entity.GetCalculation(CalculationType.ActivePower).GlobalId;
                analogMeasurementToEntityMap.Add(analongMeasurementGid, entity.GlobalId);

                locker.ExitWriteLock();
            }
            else
            {
                Logger.Instance.Log($"[{GetType()}] Entity with type {entityType} cannot be added into this storage.");
            }

            return added;
        }

        public object Clone()
        {
            return new EnergyBalanceStorage();
        }

        public bool EntityExists(long globalId)
        {
            DMSType entityType = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(globalId);
            switch (entityType)
            {
                case DMSType.ENERGYCONSUMER:
                    return EnergyConsumerStorage.EntityExists(globalId);
                case DMSType.ENERGYSTORAGE:
                case DMSType.SOLARGENERATOR:
                case DMSType.WINDGENERATOR:
                    return EnergyGeneratorStorage.EntityExists(globalId);
                case DMSType.ENERGYSOURCE:
                    return EnergySourceStorage.EntityExists(globalId);
            }

            return false;
        }

        public List<CalculationObject> GetAllEntities()
        {
            // fix this
            return new List<CalculationObject>(0);
        }

        public CalculationObject GetEntity(long globalId)
        {
            DMSType entityType = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(globalId);
            switch (entityType)
            {
                case DMSType.ENERGYCONSUMER:
                    return EnergyConsumerStorage.GetEntity(globalId);
                case DMSType.ENERGYSTORAGE:
                case DMSType.SOLARGENERATOR:
                case DMSType.WINDGENERATOR:
                    return EnergyGeneratorStorage.GetEntity(globalId);
                case DMSType.ENERGYSOURCE:
                    return EnergySourceStorage.GetEntity(globalId);
            }

            return null;
        }

        public List<IStorageTransactionProcessor> GetStorageTransactionProcessors()
        {
            EnergyGeneratorStorageItemCreator energyGeneratorStorageItemCreator = new EnergyGeneratorStorageItemCreator();
            Dictionary<DMSType, IStorageItemCreator> storageItemCreators = new Dictionary<DMSType, IStorageItemCreator>()
            {
                { DMSType.WINDGENERATOR, energyGeneratorStorageItemCreator },
                { DMSType.SOLARGENERATOR, energyGeneratorStorageItemCreator },
                { DMSType.ENERGYSTORAGE, energyGeneratorStorageItemCreator },
                { DMSType.ENERGYCONSUMER, new EnergyConsumerStorageItemCreator() },
                { DMSType.ENERGYSOURCE, new EnergySourceStorageItemCreator() }
            };

            return new List<IStorageTransactionProcessor>(1) { new EnergyBalanceStorageTransactionProcessor(this, storageItemCreators) };
        }

        public void ShallowCopyEntities(IStorage<CalculationObject> storage)
        {
            EnergyBalanceStorage copyStorage = storage as EnergyBalanceStorage;

            EnergyConsumerStorage.ShallowCopyEntities(copyStorage.EnergyConsumerStorage);
            EnergyGeneratorStorage.ShallowCopyEntities(copyStorage.EnergyGeneratorStorage);
            EnergySourceStorage.ShallowCopyEntities(copyStorage.EnergySourceStorage);

            locker.EnterWriteLock();
            copyStorage.locker.EnterReadLock();

            foreach (var analogToEntityMap in copyStorage.analogMeasurementToEntityMap)
            {
                analogMeasurementToEntityMap.Add(analogToEntityMap.Key, analogToEntityMap.Value);
            }

            copyStorage.locker.ExitReadLock();
            locker.ExitWriteLock();
        }

        public bool ValidateEntity(CalculationObject entity)
        {
            return entity != null;
        }

        public long GetEntityByAnalogMeasurementGid(long analogMeasurementGid)
        {
            locker.EnterReadLock();
            long entityGid = 0;

            analogMeasurementToEntityMap.TryGetValue(analogMeasurementGid, out entityGid);

            locker.ExitReadLock();

            return entityGid;
        }

        public void UpdateEntityProperty(long entityGid, Predicate<CalculationObject> predicate)
        {
            throw new NotImplementedException();
        }

        public IStorage<EnergyConsumer> EnergyConsumerStorage { get; private set; }

        public IStorage<EnergyGenerator> EnergyGeneratorStorage { get; private set; }

        public IStorage<EnergySource> EnergySourceStorage { get; private set; }
    }
}
