using CalculationEngine.Model.DERStates;
using Common.ComponentStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.AbstractModel;
using Common.ComponentStorage.StorageItemCreator;

namespace CalculationEngine.TransactionProcessing.StorageTransactionProcessor.DERState
{
    class DERStateStorageTransactionProcessor : StorageTransactionProcessor<Model.DERStates.DERState>
    {
        public DERStateStorageTransactionProcessor(IStorage<Model.DERStates.DERState> storage, Dictionary<DMSType, IStorageItemCreator> storageItemCreators) : base(storage, storageItemCreators)
        {
        }

        protected override List<DMSType> GetPrimaryTypes()
        {
            return new List<DMSType>()
            {
                DMSType.ENERGYSTORAGE,
                DMSType.WINDGENERATOR,
                DMSType.SOLARGENERATOR
            };
        }
    }
}
