using System.Collections.Generic;
using Common.AbstractModel;
using Common.ComponentStorage;
using Common.GDA;
using UIAdapter.Model.DERGroup;
using Common.ComponentStorage.StorageItemCreator;

namespace UIAdapter.TransactionProcessing.StorageItemCreators.DERGroup
{
    public class EnergyStorageStorageItemCreator : StorageItemCreator
    {
        public EnergyStorageStorageItemCreator()
        {
        }

        public override IdentifiedObject CreateStorageItem(ResourceDescription rd, Dictionary<DMSType, List<ResourceDescription>> affectedEntities)
        {
            EnergyStorage energyStorage = new EnergyStorage(rd.Id);
            energyStorage.NominalPower = rd.GetProperty(ModelCode.DER_NOMINALPOWER).AsFloat();
            energyStorage.Capacity = rd.GetProperty(ModelCode.ENERGYSTORAGE_CAPACITY).AsFloat();
            energyStorage.Name = rd.GetProperty(ModelCode.IDOBJ_NAME).AsString();

            PopulateStorageStateOfCharge(energyStorage, rd, affectedEntities);

            return energyStorage;
        }

        public override Dictionary<DMSType, List<ModelCode>> GetNeededProperties()
        {
            return new Dictionary<DMSType, List<ModelCode>>()
            {
                {
                    DMSType.ENERGYSTORAGE,
                    new List<ModelCode>()
                    {
                        ModelCode.IDOBJ_NAME,
                        ModelCode.DER_NOMINALPOWER,
                        ModelCode.ENERGYSTORAGE_CAPACITY,
                        ModelCode.ENERGYSTORAGE_GENERATOR,
                        ModelCode.PSR_MEASUREMENTS }
                },
                {
                    DMSType.MEASUREMENTANALOG,
                    new List<ModelCode>() { ModelCode.MEASUREMENT_MEASUREMENTYPE, ModelCode.MEASUREMENTANALOG_CURRENTVALUE }
                },
            };
        }

        private void PopulateStorageStateOfCharge(EnergyStorage energyStorage, ResourceDescription rd, Dictionary<DMSType, List<ResourceDescription>> affectedEntities)
        {
            List<long> measurementGids = rd.GetProperty(ModelCode.PSR_MEASUREMENTS).AsReferences();

            foreach (var analogMeasuremetRd in affectedEntities[DMSType.MEASUREMENTANALOG])
            {
                if (!measurementGids.Contains(analogMeasuremetRd.Id))
                {
                    continue;
                }

                MeasurementType measurementType = (MeasurementType)analogMeasuremetRd.GetProperty(ModelCode.MEASUREMENT_MEASUREMENTYPE).AsEnum();

                if (measurementType == MeasurementType.Percent)
                {
                    energyStorage.StateOfChargeMeasurementGid = analogMeasuremetRd.Id;
                    energyStorage.StateOfCharge = analogMeasuremetRd.GetProperty(ModelCode.MEASUREMENTANALOG_CURRENTVALUE).AsFloat();
                    return;
                }
            }
        }
    }
}
