using CalculationEngine.Model.EnergyCalculations;
using Common.ComponentStorage;
using System.Collections.Generic;

namespace CalculationEngine.TransactionProcessing.Storage.EnergyBalance
{
    public class EnergyConsumerStorage : Storage<EnergyConsumer>
    {
        public EnergyConsumerStorage() : base("EnergyBalance: Energy consumer storage")
        {
        }

        public override List<IStorageTransactionProcessor> GetStorageTransactionProcessors()
        {
            return new List<IStorageTransactionProcessor>(0);
        }

        protected override IStorage<EnergyConsumer> CreateNewStorage()
        {
            return new EnergyConsumerStorage();
        }
    }
}
