using Common.AbstractModel;
using Common.ComponentStorage;
using Common.ComponentStorage.StorageItemCreator;
using Common.GDA;
using FieldProcessor.Model;
using FieldProcessor.ValueExtractor;
using System.Collections.Generic;
using System.Linq;

namespace FieldProcessor.TransactionProcessing.TransactionProcessors
{
    public class DiscreteTransactionProcessor : StorageTransactionProcessor<RemotePoint>
    {
        private RemotePointAddressCollector remotePointAddressCollector;

        public DiscreteTransactionProcessor(IStorage<RemotePoint> storage, Dictionary<DMSType, IStorageItemCreator> storageItemCreators,
            RemotePointAddressCollector remotePointAddressCollector) : base(storage, storageItemCreators)
        {
            this.remotePointAddressCollector = remotePointAddressCollector;
        }

        protected override List<DMSType> GetPrimaryTypes()
        {
            return new List<DMSType>() { DMSType.MEASUREMENTDISCRETE };
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
