using Common.ComponentStorage.StorageItemCreator;
using System.Collections.Generic;
using System.Linq;
using Common.AbstractModel;
using Common.ComponentStorage;
using Common.GDA;
using UIAdapter.Model.Schema;
using Common.Logger;

namespace UIAdapter.TransactionProcessing.StorageItemCreators.Schema
{
    public class BreakerStateStorageItemCreator : StorageItemCreator
    {
        public BreakerStateStorageItemCreator() : base()
        {
        }

        public override IdentifiedObject CreateStorageItem(ResourceDescription rd, Dictionary<DMSType, List<ResourceDescription>> affectedEntities)
        {
            ResourceDescription measurementDiscrete = FindDiscreteRemotePoint(rd, affectedEntities);

            if (measurementDiscrete == null)
            {
                Logger.Instance.Log($"[{this.GetType()}] Couldn't find discrete remote point. Skipping breaker with gid {rd.Id:X16}!");
                return null;
            }

            Breaker breakerState = new Breaker(rd.Id);
            breakerState.MeasurementDiscreteGid = measurementDiscrete.Id;
            breakerState.CurrentValue = measurementDiscrete.GetProperty(ModelCode.MEASUREMENTDISCRETE_CURRENTVALUE).AsInt();

            return breakerState;
        }

        public override Dictionary<DMSType, List<ModelCode>> GetNeededProperties()
        {
            return new Dictionary<DMSType, List<ModelCode>>()
            {
                { DMSType.BREAKER, new List<ModelCode>() { ModelCode.PSR_MEASUREMENTS } },
                { DMSType.MEASUREMENTDISCRETE, new List<ModelCode>() { ModelCode.MEASUREMENTDISCRETE_CURRENTVALUE } }
            };
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
    }
}
