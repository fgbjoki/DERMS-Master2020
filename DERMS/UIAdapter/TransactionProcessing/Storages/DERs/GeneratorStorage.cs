using Common.AbstractModel;
using Common.ComponentStorage;
using Common.ComponentStorage.StorageItemCreator;
using System.Collections.Generic;
using UIAdapter.Model.DERs;
using UIAdapter.TransactionProcessing.StorageItemCreators.DERs;
using UIAdapter.TransactionProcessing.StorageTransactionProcessors.DERs;

namespace UIAdapter.TransactionProcessing.Storages.DERs
{
    public class GeneratorStorage : Storage<Generator>
    {
        public GeneratorStorage() : base("DER: Generator storage")
        {
        }

        public override List<IStorageTransactionProcessor> GetStorageTransactionProcessors()
        {
            Dictionary<DMSType, IStorageItemCreator> itemCreator = new Dictionary<DMSType, IStorageItemCreator>()
            {
                { DMSType.WINDGENERATOR, new WindGeneratorStorageItemCreator() },
                { DMSType.SOLARGENERATOR, new SolarPanelStorageItemCreator() }
            };

            return new List<IStorageTransactionProcessor>(1) { new GeneratorStorageTransactionProcessor(this, itemCreator) };
        }

        protected override IStorage<Generator> CreateNewStorage()
        {
            return new GeneratorStorage();
        }
    }
}
