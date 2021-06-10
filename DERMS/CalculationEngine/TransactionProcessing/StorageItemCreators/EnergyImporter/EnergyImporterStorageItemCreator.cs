using Common.ComponentStorage.StorageItemCreator;
using System.Collections.Generic;
using Common.AbstractModel;
using Common.ComponentStorage;
using Common.GDA;
using CalculationEngine.Model.EnergyImporter;
using System.Linq;

namespace CalculationEngine.TransactionProcessing.StorageItemCreators.EnergyImporter
{
    public class EnergyImporterStorageItemCreator : StorageItemCreator
    {
        public override IdentifiedObject CreateStorageItem(ResourceDescription rd, Dictionary<DMSType, List<ResourceDescription>> affectedEntities)
        {
            EnergySource energySource = new EnergySource(rd.Id);
            ResourceDescription analogMeasurement = affectedEntities[DMSType.MEASUREMENTANALOG].First(x => x.Id == rd.GetProperty(ModelCode.PSR_MEASUREMENTS).AsReferences().First());
            energySource.ActivePowerMeasurementGid = analogMeasurement.Id;
            energySource.ActivePower = analogMeasurement.GetProperty(ModelCode.MEASUREMENTANALOG_CURRENTVALUE).AsFloat();

            return energySource;
        }

        public override Dictionary<DMSType, List<ModelCode>> GetNeededProperties()
        {
            return new Dictionary<DMSType, List<ModelCode>>()
            {
                { DMSType.ENERGYSOURCE, new List<ModelCode>() { ModelCode.PSR_MEASUREMENTS } },
                { DMSType.MEASUREMENTANALOG, new List<ModelCode>() { ModelCode.MEASUREMENTANALOG_CURRENTVALUE } }
            };
        }
    }
}
