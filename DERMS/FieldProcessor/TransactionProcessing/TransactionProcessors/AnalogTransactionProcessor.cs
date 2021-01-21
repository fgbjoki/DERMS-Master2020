using Common.ComponentStorage;
using System.Collections.Generic;
using Common.AbstractModel;
using Common.ComponentStorage.StorageItemCreator;
using FieldProcessor.Model;
using Common.GDA;
using FieldProcessor.ValueExtractor;
using System.Linq;

namespace FieldProcessor.TransactionProcessing
{
    public class AnalogTransactionProcessor : StorageTransactionProcessor<RemotePoint>
    {
        private RemotePointAddressCollector remotePointAddressCollector;

        public AnalogTransactionProcessor(IStorage<RemotePoint> storage, Dictionary<DMSType, IStorageItemCreator> storageItemCreators, 
            RemotePointAddressCollector remotePointAddressCollector) : base(storage, storageItemCreators)
        {
            this.remotePointAddressCollector = remotePointAddressCollector;
        }

        protected override List<DMSType> GetPrimaryTypes()
        {
            return new List<DMSType>() { DMSType.MEASUREMENTANALOG };
        }

        public override bool Commit()
        {
            bool commited = base.Commit();

            if (commited == false)
            {
                return false;
            }

            remotePointAddressCollector.Commit();

            return commited;
        }

        public override bool Rollback()
        {
            remotePointAddressCollector.Rollback();

            return base.Rollback();
        }

        protected override bool AdditionalProcessing(Dictionary<DMSType, List<ResourceDescription>> affectedEntities)
        {
            bool isProcessingValid = base.AdditionalProcessing(affectedEntities);

            if (isProcessingValid == false)
            {
                return false;
            }

            return AdditionalProcessing();
        }

        private bool AdditionalProcessing()
        {
            remotePointAddressCollector.Prepare(preparedObjects.Values.ToList());

            return true;
        }
    }
}
