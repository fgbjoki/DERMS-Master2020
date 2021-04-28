using Common.ComponentStorage.StorageItemCreator;
using System.Collections.Generic;
using Common.AbstractModel;
using Common.ComponentStorage;
using Common.GDA;
using CalculationEngine.Model.DERStates;

namespace CalculationEngine.TransactionProcessing.StorageItemCreators.DERstates
{
    public class DERStateStorageItemCreator : IStorageItemCreator
    {
        public IdentifiedObject CreateStorageItem(ResourceDescription rd, Dictionary<DMSType, List<ResourceDescription>> affectedEntities)
        {
            DERState derState = new DERState(rd.Id);
            PopulateActivePowerMeasurement(derState, rd, affectedEntities);

            return derState;
        }

        public Dictionary<DMSType, List<ModelCode>> GetNeededProperties()
        {
            return new Dictionary<DMSType, List<ModelCode>>()
            {
                {
                    DMSType.ENERGYSTORAGE,
                    new List<ModelCode>(1) { ModelCode.PSR_MEASUREMENTS }
                },
                {
                    DMSType.WINDGENERATOR,
                    new List<ModelCode>(1) { ModelCode.PSR_MEASUREMENTS }
                },
                {
                    DMSType.SOLARGENERATOR,
                    new List<ModelCode>(1) { ModelCode.PSR_MEASUREMENTS }
                },
                {
                    DMSType.MEASUREMENTANALOG,
                    new List<ModelCode>() { ModelCode.MEASUREMENT_MEASUREMENTYPE, ModelCode.MEASUREMENTANALOG_CURRENTVALUE }
                }
            };
        }

        private void PopulateActivePowerMeasurement(DERState derState, ResourceDescription rd, Dictionary<DMSType, List<ResourceDescription>> affectedEntities)
        {
            List<long> measurementGids = rd.GetProperty(ModelCode.PSR_MEASUREMENTS).AsReferences();

            foreach (var analogMeasuremetRd in affectedEntities[DMSType.MEASUREMENTANALOG])
            {
                if (!measurementGids.Contains(analogMeasuremetRd.Id))
                {
                    continue;
                }

                MeasurementType measurementType = (MeasurementType)analogMeasuremetRd.GetProperty(ModelCode.MEASUREMENT_MEASUREMENTYPE).AsEnum();

                if (measurementType == MeasurementType.ActivePower)
                {
                    derState.ActivePowerMeasurementGid = analogMeasuremetRd.Id;
                    derState.ActivePower = analogMeasuremetRd.GetProperty(ModelCode.MEASUREMENTANALOG_CURRENTVALUE).AsFloat();
                    return;
                }
            }
        }
    }
}
