using Common.ComponentStorage;
using System.Collections.Generic;
using Common.AbstractModel;
using Common.ComponentStorage.StorageItemCreator;
using FieldProcessor.Model;
using Common.GDA;
using System.Linq;
using FieldProcessor.TransactionProcessing.TransactionProcessors;

namespace FieldProcessor.TransactionProcessing
{
    public class AnalogTransactionProcessor : StorageTransactionProcessor<RemotePoint>
    {
        private List<IRemotePointDependentTransactionUnit> remotePointDependentUnits;

        public AnalogTransactionProcessor(IStorage<RemotePoint> storage, Dictionary<DMSType, IStorageItemCreator> storageItemCreators,
            params IRemotePointDependentTransactionUnit[] remotePointDependentUnits) : base(storage, storageItemCreators)
        {
            this.remotePointDependentUnits = new List<IRemotePointDependentTransactionUnit>(remotePointDependentUnits);
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

            remotePointDependentUnits.ForEach(x => x.Commit());
            remotePointDependentUnits.ForEach(x => x.RelaseTransactionResources());

            return commited;
        }

        public override bool Rollback()
        {
            remotePointDependentUnits.ForEach(x => x.Rollback());
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
            remotePointDependentUnits.ForEach(x => x.Prepare(preparedObjects.Values.ToList()));

            return true;
        }
    }
}
