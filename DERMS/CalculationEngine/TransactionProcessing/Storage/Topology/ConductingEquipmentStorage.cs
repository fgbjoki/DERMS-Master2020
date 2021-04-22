using CalculationEngine.Model.Topology;
using CalculationEngine.Model.Topology.Transaction;
using CalculationEngine.TransactionProcessing.StorageTransactionProcessor.Topology;
using Common.ComponentStorage;
using Common.Helpers.Breakers;
using System.Collections.Generic;

namespace CalculationEngine.TransactionProcessing.Storage.Topology
{
    public class ConductingEquipmentStorage : Storage<ConductingEquipment>
    {
        private ReferenceResolver referenceResolver;
        private BreakerMessageMapping breakerMessageMapping;

        public ConductingEquipmentStorage(ReferenceResolver referenceResolver, BreakerMessageMapping breakerMessageMapping) : base("Topology:ConductingEquipmentStorage")
        {
            this.referenceResolver = referenceResolver;
            this.breakerMessageMapping = breakerMessageMapping;
        }

        public override List<IStorageTransactionProcessor> GetStorageTransactionProcessors()
        {
            return new List<IStorageTransactionProcessor>();
        }

        protected override IStorage<ConductingEquipment> CreateNewStorage()
        {
            return new ConductingEquipmentStorage(referenceResolver, breakerMessageMapping);
        }
    }
}
