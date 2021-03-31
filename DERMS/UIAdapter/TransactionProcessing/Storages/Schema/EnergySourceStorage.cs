using Common.AbstractModel;
using Common.ComponentStorage;
using Common.ComponentStorage.StorageItemCreator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIAdapter.Model.Schema;
using UIAdapter.TransactionProcessing.StorageItemCreators.Schema;
using UIAdapter.TransactionProcessing.StorageTransactionProcessors.Schema;

namespace UIAdapter.TransactionProcessing.Storages.Schema
{
    public class EnergySourceStorage : Storage<EnergySource>
    {
        public EnergySourceStorage() : base("Schema energy storage")
        {
        }

        public override List<IStorageTransactionProcessor> GetStorageTransactionProcessors()
        {
            Dictionary<DMSType, IStorageItemCreator> itemCreators = new Dictionary<DMSType, IStorageItemCreator>()
            {
                { DMSType.ENERGYSOURCE, new EnergySourceItemCreator() }
            };

            return new List<IStorageTransactionProcessor>(1) { new EnergySourceTransactionProcessor(this, itemCreators) };
        }

        protected override IStorage<EnergySource> CreateNewStorage()
        {
            return new EnergySourceStorage();
        }
    }
}
