using CalculationEngine.Model.DERCommanding;
using System.Collections.Generic;
using Common.GDA;
using Common.AbstractModel;

namespace CalculationEngine.TransactionProcessing.StorageItemCreators.DERCommanding
{
    public class EnergyStorageItemCreator : DistributedEnergyResourceStorageItemCreator<EnergyStorage>
    {
        public override Dictionary<DMSType, List<ModelCode>> GetNeededProperties()
        {
            var neededProperties = base.GetNeededProperties();

            List<ModelCode> energyStorageProperties;
            if (!neededProperties.TryGetValue(DMSType.ENERGYSTORAGE, out energyStorageProperties))
            {
                energyStorageProperties = new List<ModelCode>();
                neededProperties.Add(DMSType.ENERGYSTORAGE, energyStorageProperties);
            }

            energyStorageProperties.Add(ModelCode.PSR_MEASUREMENTS);
            energyStorageProperties.Add(ModelCode.ENERGYSTORAGE_CAPACITY);
            return neededProperties;
        }

        protected override EnergyStorage InstantiateDER(ResourceDescription rd)
        {
            return new EnergyStorage(rd.Id);
        }

        protected override void PopulateDERProperties(EnergyStorage der, ResourceDescription rd, Dictionary<DMSType, List<ResourceDescription>> affectedEntities)
        {
            base.PopulateDERProperties(der, rd, affectedEntities);
            der.Capacity = rd.GetProperty(ModelCode.ENERGYSTORAGE_CAPACITY).AsFloat();

            PopulateMeasurementProperties(der, rd, affectedEntities);
        }

        private void PopulateMeasurementProperties(EnergyStorage der, ResourceDescription rd, Dictionary<DMSType, List<ResourceDescription>> affectedEntities)
        {
            PopulateStorageStateOfCharge(der, rd, affectedEntities);
            PopulateActivePower(der, rd, affectedEntities);
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

        private void PopulateActivePower(EnergyStorage energyStorage, ResourceDescription rd, Dictionary<DMSType, List<ResourceDescription>> affectedEntities)
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
                    energyStorage.ActivePowerMeasurementGid = analogMeasuremetRd.Id;
                    return;
                }
            }
        }

    }
}
