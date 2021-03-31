using CalculationEngine.Model.Topology.Transaction;
using CalculationEngine.TransactionProcessing.StorageTransactionProcessor.Topology;
using Common.ComponentStorage;
using System.Collections.Generic;

namespace CalculationEngine.TransactionProcessing.Storage.Topology
{
    public class ConnectivityNodeStorage : Storage<ConnectivityNode>
    {
        private ReferenceResolver referenceResolver;

        public ConnectivityNodeStorage(ReferenceResolver referenceResolver) : base("Topology:ConnectivityNodeStorage")
        {
            this.referenceResolver = referenceResolver;
        }

        public override List<IStorageTransactionProcessor> GetStorageTransactionProcessors()
        {
            return new List<IStorageTransactionProcessor>();
        }

        protected override IStorage<ConnectivityNode> CreateNewStorage()
        {
            return new ConnectivityNodeStorage(referenceResolver);
        }
    }
}
