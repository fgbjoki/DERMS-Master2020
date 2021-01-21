using Common.AbstractModel;
using Common.ComponentStorage;
using Common.ComponentStorage.StorageItemCreator;
using Common.Logger;
using Common.ServiceLocator;
using FieldProcessor.Model;
using FieldProcessor.TransactionProcessing.StorageItemCreators;
using FieldProcessor.TransactionProcessing.TransactionProcessors;
using FieldProcessor.ValueExtractor;
using System.Collections.Generic;

namespace FieldProcessor.TransactionProcessing.Storages
{
    public class DiscreteRemotePointStorage : Storage<RemotePoint>
    {
        private HashSet<ushort> usedAddresses;

        public DiscreteRemotePointStorage() : base("Discrete remote point storage")
        {
            usedAddresses = new HashSet<ushort>();
        }

        public override List<IStorageTransactionProcessor> GetStorageTransactionProcessors()
        {
            var storageItemCreators = new Dictionary<DMSType, IStorageItemCreator>()
            {
                { DMSType.MEASUREMENTDISCRETE, new DiscreteStorageItemCreator() }
            };

            RemotePointAddressCollector remotePointAddressCollector = ServiceLocator.GetService<RemotePointAddressCollector>();

            return new List<IStorageTransactionProcessor>() { new DiscreteTransactionProcessor(this, storageItemCreators, remotePointAddressCollector) };
        }

        public override bool AddEntity(RemotePoint item)
        {
            if (item == null)
            {
                DERMSLogger.Instance.Log($"[{storageName}] Cannot add null to storage!");
                return false;
            }

            if (!usedAddresses.Add(item.Address))
            {
                DERMSLogger.Instance.Log($"[{storageName}] Remote point with address: {item.Address} already exists!");
                return false;
            }

            bool addSucceded = base.AddEntity(item);

            if (!addSucceded)
            {
                usedAddresses.Remove(item.Address);
            }

            return addSucceded;
        }

        public override bool ValidateEntity(RemotePoint entity)
        {
            return entity != null && !usedAddresses.Contains(entity.Address) &&
                (entity.Type == RemotePointType.Coil || entity.Type == RemotePointType.DiscreteInput);
        }
    }
}
