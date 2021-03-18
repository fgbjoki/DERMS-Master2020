using System.Collections.Generic;
using Common.AbstractModel;
using Common.ComponentStorage;
using Common.GDA;
using CalculationEngine.TransactionProcessing.StorageTransactionProcessor.Topology;
using CalculationEngine.Model.Topology.Transaction;
using Common.Logger;

namespace CalculationEngine.TransactionProcessing.StorageItemCreators.Topology
{
    public class DERGeneratorStorageItemCreator : ConductingEquipmentStorageItemCreator
    {
        public DERGeneratorStorageItemCreator(ReferenceResolver referenceResolver) : base(CreatePropertiesPerType(), referenceResolver)
        {
        }

        public override IdentifiedObject CreateStorageItem(ResourceDescription rd, Dictionary<DMSType, List<ResourceDescription>> affectedEntities)
        {
            long energyStorageGid = rd.GetProperty(ModelCode.GENERATOR_ENERGYSTORAGE).AsReference();
            EnergyStorage energyStorage = referenceResolver.GetConductingEquipment(energyStorageGid) as EnergyStorage;

            if (energyStorage == null)
            {
                Logger.Instance.Log($"[{this.GetType().Name}] Couldn't find energy storage with gid: {energyStorageGid:X16}. Skipping generator with gid {rd.Id:X16}!");
                return null;
            }

            Generator newGenerator = base.CreateStorageItem(rd, affectedEntities) as Generator;
            if (newGenerator == null)
            {
                return null;
            }

            return newGenerator;
        }

        protected override ConductingEquipment CreateEntity(ResourceDescription rd)
        {
            return new Generator(rd.Id);
        }

        private static Dictionary<DMSType, List<ModelCode>> CreatePropertiesPerType()
        {
            return new Dictionary<DMSType, List<ModelCode>>()
            {
                { DMSType.SOLARGENERATOR, new List<ModelCode>() { ModelCode.CONDUCTINGEQ_TERMINALS, ModelCode.GENERATOR_ENERGYSTORAGE } },
                { DMSType.WINDGENERATOR, new List<ModelCode>() { ModelCode.CONDUCTINGEQ_TERMINALS, ModelCode.GENERATOR_ENERGYSTORAGE } }
            };
        }
    }
}
