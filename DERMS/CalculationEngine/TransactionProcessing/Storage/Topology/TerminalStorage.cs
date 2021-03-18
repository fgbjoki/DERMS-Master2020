using CalculationEngine.Model.Topology.Transaction;
using CalculationEngine.TransactionProcessing.StorageTransactionProcessor.Topology;
using Common.ComponentStorage;
using System.Collections.Generic;
using System;

namespace CalculationEngine.TransactionProcessing.Storage.Topology
{
    public class TerminalStorage : Storage<Terminal>
    {
        private ReferenceResolver referenceResolver;

        public TerminalStorage(ReferenceResolver referenceResolver) : base("Topology:TerminalStorage")
        {
            this.referenceResolver = referenceResolver;
        }

        public override List<IStorageTransactionProcessor> GetStorageTransactionProcessors()
        {
            return new List<IStorageTransactionProcessor>() {};
        }

        protected override IStorage<Terminal> CreateNewStorage()
        {
            return new TerminalStorage(referenceResolver);
        }
    }
}
