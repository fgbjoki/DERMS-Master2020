using CalculationEngine.Model.Topology.Transaction;
using CalculationEngine.TransactionProcessing.StorageTransactionProcessor.Topology;
using Common.ComponentStorage;
using System.Collections.Generic;
using System;

namespace CalculationEngine.TransactionProcessing.Storage.Topology
{
    public class EnergySourceStorage : Storage<EnergySource>
    {
        private ReferenceResolver referenceResolver;

        public EnergySourceStorage(ReferenceResolver referenceResolver) : base("Topology:EnergySourceStorage")
        {
            this.referenceResolver = referenceResolver;
        }

        public override List<IStorageTransactionProcessor> GetStorageTransactionProcessors()
        {
            return new List<IStorageTransactionProcessor>();
        }

        protected override IStorage<EnergySource> CreateNewStorage()
        {
            return new EnergySourceStorage(referenceResolver);
        }
    }
}
