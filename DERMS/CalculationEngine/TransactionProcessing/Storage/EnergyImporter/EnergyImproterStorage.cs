using CalculationEngine.Model.EnergyImporter;
using CalculationEngine.TransactionProcessing.StorageItemCreators.EnergyImporter;
using CalculationEngine.TransactionProcessing.StorageTransactionProcessor.EnergyImporter;
using Common.AbstractModel;
using Common.ComponentStorage;
using Common.ComponentStorage.StorageItemCreator;
using System.Collections.Generic;

namespace CalculationEngine.TransactionProcessing.Storage.EnergyImporter
{
    public class EnergyImproterStorage : Storage<EnergySource>
    {
        public EnergyImproterStorage() : base("Energy importer storage")
        {
        }

        public override List<IStorageTransactionProcessor> GetStorageTransactionProcessors()
        {
            Dictionary<DMSType, IStorageItemCreator> creator = new Dictionary<DMSType, IStorageItemCreator>()
            {
                { DMSType.ENERGYSOURCE, new EnergyImporterStorageItemCreator() }
            };

            return new List<IStorageTransactionProcessor>(1) { new EnergyImporterStorageTransactionProcessor(this, creator) };
        }

        protected override IStorage<EnergySource> CreateNewStorage()
        {
            return new EnergyImproterStorage();
        }
    }
}
