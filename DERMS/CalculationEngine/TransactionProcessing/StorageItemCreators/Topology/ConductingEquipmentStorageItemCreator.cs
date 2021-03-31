using Common.ComponentStorage.StorageItemCreator;
using System.Collections.Generic;
using Common.AbstractModel;
using Common.ComponentStorage;
using Common.GDA;
using CalculationEngine.Model.Topology.Transaction;
using CalculationEngine.TransactionProcessing.StorageTransactionProcessor.Topology;
using Common.Logger;

namespace CalculationEngine.TransactionProcessing.StorageItemCreators.Topology
{
    public class ConductingEquipmentStorageItemCreator : StorageItemCreator
    {
        protected ReferenceResolver referenceResolver;

        public ConductingEquipmentStorageItemCreator(Dictionary<DMSType, List<ModelCode>> propertiesPerType, ReferenceResolver referenceResolver) : base(propertiesPerType)
        {
            this.referenceResolver = referenceResolver;
        }

        public ConductingEquipmentStorageItemCreator(ReferenceResolver referenceResolver) : base(CreatePropertiesPerType())
        {
            this.referenceResolver = referenceResolver;
        }

        public override IdentifiedObject CreateStorageItem(ResourceDescription rd, Dictionary<DMSType, List<ResourceDescription>> affectedEntities)
        {
            ConductingEquipment conductingEquipment = CreateEntity(rd);

            if (!ConnectConductingEquipmentToTerminals(conductingEquipment, rd.GetProperty(ModelCode.CONDUCTINGEQ_TERMINALS).AsReferences()))
            {
                return null;
            }

            referenceResolver.AddReference(conductingEquipment);

            return conductingEquipment;
        }

        protected virtual ConductingEquipment CreateEntity(ResourceDescription rd)
        {
            return new ConductingEquipment(rd.Id);
        }

        protected bool ConnectConductingEquipmentToTerminals(ConductingEquipment conductingEquipment, List<long> terminalGids)
        {
            foreach (var terminalGid in terminalGids)
            {
                Terminal terminal = referenceResolver.GetTerminal(terminalGid);

                if (terminal == null)
                {
                    Logger.Instance.Log($"[{this.GetType().Name}] Couldn't find terminal with gid: {terminal:X16}. Skipping conducting equipment with gid {conductingEquipment.GlobalId:X16}!");
                    return false;
                }

                conductingEquipment.ConnectToTerminal(terminal);
            }

            return true;
        }

        private static Dictionary<DMSType, List<ModelCode>> CreatePropertiesPerType()
        {
            return new Dictionary<DMSType, List<ModelCode>>()
            {
                { DMSType.ACLINESEG, new List<ModelCode>() { ModelCode.CONDUCTINGEQ_TERMINALS } },
                { DMSType.ENERGYCONSUMER, new List<ModelCode>() { ModelCode.CONDUCTINGEQ_TERMINALS } },
            };
        }
    }
}
