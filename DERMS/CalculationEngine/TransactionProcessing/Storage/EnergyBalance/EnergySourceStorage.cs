using CalculationEngine.Model.EnergyCalculations;
using Common.ComponentStorage;
using System.Collections.Generic;

namespace CalculationEngine.TransactionProcessing.Storage.EnergyBalance
{
    public class EnergySourceStorage : Storage<EnergySource>
    {
        public EnergySourceStorage() : base("EnergyBalance: Energy source storage")
        {
        }

        public override List<IStorageTransactionProcessor> GetStorageTransactionProcessors()
        {
            return new List<IStorageTransactionProcessor>(0);
        }

        protected override IStorage<EnergySource> CreateNewStorage()
        {
            return new EnergySourceStorage();
        }
    }
}
