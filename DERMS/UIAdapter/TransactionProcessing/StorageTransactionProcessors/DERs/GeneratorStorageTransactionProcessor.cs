using Common.ComponentStorage;
using System.Collections.Generic;
using UIAdapter.Model.DERs;
using Common.AbstractModel;
using Common.ComponentStorage.StorageItemCreator;

namespace UIAdapter.TransactionProcessing.StorageTransactionProcessors.DERs
{
    public class GeneratorStorageTransactionProcessor : StorageTransactionProcessor<Generator>
    {
        public GeneratorStorageTransactionProcessor(IStorage<Generator> storage, Dictionary<DMSType, IStorageItemCreator> storageItemCreators) : base(storage, storageItemCreators)
        {
        }

        protected override List<DMSType> GetPrimaryTypes()
        {
            return new List<DMSType>(2) { DMSType.WINDGENERATOR, DMSType.SOLARGENERATOR };
        }
    }
}
