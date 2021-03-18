using System.Collections.Generic;
using CalculationEngine.TransactionProcessing.StorageTransactionProcessor.Topology;
using Common.AbstractModel;
using Common.GDA;
using CalculationEngine.Model.Topology.Transaction;

namespace CalculationEngine.TransactionProcessing.StorageItemCreators.Topology
{
    public class EnergyStorageStorageItemCreator : ConductingEquipmentStorageItemCreator
    {
        public EnergyStorageStorageItemCreator(ReferenceResolver referenceResolver) : base(CreatePropertiesPerType(), referenceResolver)
        {
        }

        protected override ConductingEquipment CreateEntity(ResourceDescription rd)
        {
            return new EnergyStorage(rd.Id);
        }

        private static Dictionary<DMSType, List<ModelCode>> CreatePropertiesPerType()
        {
            return new Dictionary<DMSType, List<ModelCode>>()
            {
                { DMSType.ENERGYSTORAGE, new List<ModelCode>() { ModelCode.CONDUCTINGEQ_TERMINALS } }
            };
        }
    }
}
