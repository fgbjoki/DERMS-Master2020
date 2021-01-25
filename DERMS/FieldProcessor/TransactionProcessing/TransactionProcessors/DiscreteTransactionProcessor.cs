using Common.AbstractModel;
using Common.ComponentStorage;
using Common.ComponentStorage.StorageItemCreator;
using Common.GDA;
using FieldProcessor.Model;
using System.Collections.Generic;
using System.Linq;

namespace FieldProcessor.TransactionProcessing.TransactionProcessors
{
    public class DiscreteTransactionProcessor : StorageTransactionProcessor<RemotePoint>
    {
        private List<IRemotePointDependentTransactionUnit> remotePointDependentUnits;

        public DiscreteTransactionProcessor(IStorage<RemotePoint> storage, Dictionary<DMSType, IStorageItemCreator> storageItemCreators,
            params IRemotePointDependentTransactionUnit[] remotePointDependentUnits) : base(storage, storageItemCreators)
        {
            this.remotePointDependentUnits = new List<IRemotePointDependentTransactionUnit>(remotePointDependentUnits);
        }

        protected override List<DMSType> GetPrimaryTypes()
        {
            return new List<DMSType>() { DMSType.MEASUREMENTDISCRETE };
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
