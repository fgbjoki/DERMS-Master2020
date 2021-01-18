using Common.ComponentStorage;
using FieldProcessor.Model;
using System.Collections.Generic;
using Common.AbstractModel;
using Common.ComponentStorage.StorageItemCreator;
using FieldProcessor.TransactionProcessing.StorageItemCreators;
using Common.Logger;

namespace FieldProcessor.TransactionProcessing.Storages
{
    public class AnalogRemotePointStorage : Storage<RemotePoint>
    {
        private HashSet<ushort> usedAddresses;

        public AnalogRemotePointStorage() : base("Analog Remote Point Storage")
        {
            usedAddresses = new HashSet<ushort>();
        }

        public override List<IStorageTransactionProcessor> GetStorageTransactionProcessors()
        {
            var storageItemCreators = new Dictionary<DMSType, IStorageItemCreator>()
            {
                { DMSType.MEASUREMENTANALOG, new AnalogStorageItemCreator() }
            };

            return new List<IStorageTransactionProcessor>() { new AnalogTransactionProcessor(this, storageItemCreators) };
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
                DERMSLogger.Instance.Log($"[{storageName}] Remote point with address : {item.Address} already exists!");
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
                (entity.Type == RemotePointType.HoldingRegister || entity.Type == RemotePointType.InputRegister);
        }
    }
}
