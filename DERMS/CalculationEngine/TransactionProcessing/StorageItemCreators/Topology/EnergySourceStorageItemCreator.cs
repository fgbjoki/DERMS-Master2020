using System.Collections.Generic;
using CalculationEngine.Model.Topology.Transaction;
using CalculationEngine.TransactionProcessing.StorageTransactionProcessor.Topology;
using Common.AbstractModel;
using Common.GDA;

namespace CalculationEngine.TransactionProcessing.StorageItemCreators.Topology
{
    public class EnergySourceStorageItemCreator : ConductingEquipmentStorageItemCreator
    {
        public EnergySourceStorageItemCreator(ReferenceResolver referenceResolver) : base(CreatePropertiesPerType(), referenceResolver)
        {
        }

        protected override ConductingEquipment CreateEntity(ResourceDescription rd)
        {
            return new EnergySource(rd.Id);
        }

        private static Dictionary<DMSType, List<ModelCode>> CreatePropertiesPerType()
        {
            return new Dictionary<DMSType, List<ModelCode>>()
            {
                { DMSType.ENERGYSOURCE, new List<ModelCode>() { ModelCode.CONDUCTINGEQ_TERMINALS } },
            };
        }
    }
}
