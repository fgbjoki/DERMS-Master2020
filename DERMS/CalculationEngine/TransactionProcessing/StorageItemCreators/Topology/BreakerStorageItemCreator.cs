using System.Collections.Generic;
using Common.AbstractModel;
using Common.ComponentStorage;
using Common.GDA;
using CalculationEngine.TransactionProcessing.StorageTransactionProcessor.Topology;
using CalculationEngine.Model.Topology.Transaction;
using System.Linq;
using Common.Logger;
using CalculationEngine.Model.Topology;

namespace CalculationEngine.TransactionProcessing.StorageItemCreators.Topology
{
    public class BreakerStorageItemCreator : ConductingEquipmentStorageItemCreator
    {
        private BreakerMessageMapping breakerMessageMapping;

        public BreakerStorageItemCreator(BreakerMessageMapping breakerMessageMapping, ReferenceResolver referenceResolver) : base(CreatePropertiesPerType(), referenceResolver)
        {
            this.breakerMessageMapping = breakerMessageMapping;
        }

        public override IdentifiedObject CreateStorageItem(ResourceDescription rd, Dictionary<DMSType, List<ResourceDescription>> affectedEntities)
        {
            ResourceDescription discreteRemotePoint = FindDiscreteRemotePoint(rd, affectedEntities);
            if (discreteRemotePoint == null)
            {
                Logger.Instance.Log($"[{this.GetType()}] Couldn't find discrete remote point. Skipping conducting equipment with gid {rd.Id:X16}!");
                return null;
            }

            Breaker breaker = new Breaker(rd.Id, discreteRemotePoint.Id)
            {
                State = breakerMessageMapping.MapRawDataToBreakerState(discreteRemotePoint.GetProperty(ModelCode.MEASUREMENTDISCRETE_CURRENTVALUE).AsInt())
            };

            List<long> terminalGids = rd.GetProperty(ModelCode.CONDUCTINGEQ_TERMINALS_TEMP).AsReferences();

            if (!ConnectConductingEquipmentToTerminals(breaker, terminalGids))
            {
                return null;
            }

            referenceResolver.AddReference(breaker);

            return breaker;
        }

        private ResourceDescription FindDiscreteRemotePoint(ResourceDescription rd, Dictionary<DMSType, List<ResourceDescription>> affectedEntities)
        {
            long discreteMeasurementGid = rd.GetProperty(ModelCode.PSR_MEASUREMENTS).AsReferences().First();

            List<ResourceDescription> discreteRemotePoints;
            if (!affectedEntities.TryGetValue(DMSType.MEASUREMENTDISCRETE, out discreteRemotePoints))
            {
                Logger.Instance.Log($"[{this.GetType()}] Couldn't find discrete remote point resource descriptions.");
                return null;
            }

            return discreteRemotePoints.FirstOrDefault(x => x.Id == discreteMeasurementGid);
        }

        private static Dictionary<DMSType, List<ModelCode>> CreatePropertiesPerType()
        {
            return new Dictionary<DMSType, List<ModelCode>>()
            {
                { DMSType.BREAKER, new List<ModelCode>() { ModelCode.CONDUCTINGEQ_TERMINALS_TEMP, ModelCode.PSR_MEASUREMENTS } },
                { DMSType.MEASUREMENTDISCRETE, new List<ModelCode>() { ModelCode.MEASUREMENTDISCRETE_CURRENTVALUE } }
            };
        }
    }
}
