using CalculationEngine.Model.EnergyCalculations;
using Common.ComponentStorage;
using System.Collections.Generic;

namespace CalculationEngine.TransactionProcessing.Storage.EnergyBalance
{
    public class EnergyGeneratorStorage : Storage<EnergyGenerator>
    {
        public EnergyGeneratorStorage() : base("EnergyBalance: Energy generator storage")
        {
        }

        public override List<IStorageTransactionProcessor> GetStorageTransactionProcessors()
        {
            return new List<IStorageTransactionProcessor>(0);
        }

        protected override IStorage<EnergyGenerator> CreateNewStorage()
        {
            return new EnergyGeneratorStorage();
        }
    }
}
