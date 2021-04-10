using Common.ComponentStorage.StorageItemCreator;
using System.Collections.Generic;
using Common.AbstractModel;
using Common.GDA;
using CalculationEngine.Model.EnergyCalculations;
using Common.Logger;

namespace CalculationEngine.TransactionProcessing.StorageItemCreators.EnergyBalance
{
    public abstract class BaseEnergyBalanceStorageItemCreator : StorageItemCreator
    {
        protected BaseEnergyBalanceStorageItemCreator(Dictionary<DMSType, List<ModelCode>> propertiesPerType) : base(propertiesPerType)
        {
        }

        protected bool PopulateCalculationObjectWithActivePower(ResourceDescription rd, CalculationObject calculationObject, Dictionary<DMSType, List<ResourceDescription>> affectedEntities)
        {
            ResourceDescription analogActivePowerRd = GetAnalogActivePowerResourceDescription(rd, affectedEntities);
            if (analogActivePowerRd == null)
            {
                Logger.Instance.Log($"[{GetType()}] Cannot find cooresponding active power for energy consumer with gid: {rd.Id:X16}.");
                return false;
            }

            float currentActivePower = analogActivePowerRd.GetProperty(ModelCode.MEASUREMENTANALOG_CURRENTVALUE).AsFloat();

            calculationObject.AddCalculation(analogActivePowerRd.Id, CalculationType.ActivePower, currentActivePower);

            return true;
        } 

        protected ResourceDescription GetAnalogActivePowerResourceDescription(ResourceDescription rd, Dictionary<DMSType, List<ResourceDescription>> affectedEntities)
        {
            List<long> specifiedMeasurements = rd.GetProperty(ModelCode.PSR_MEASUREMENTS).AsReferences();

            foreach (var analogMeasurement in affectedEntities[DMSType.MEASUREMENTANALOG])
            {
                if (!specifiedMeasurements.Contains(analogMeasurement.Id))
                {
                    continue;
                }

                MeasurementType measurementType = (MeasurementType)analogMeasurement.GetProperty(ModelCode.MEASUREMENT_MEASUREMENTYPE).AsEnum();

                if (measurementType != MeasurementType.ActivePower)
                {
                    continue;
                }

                return analogMeasurement;
            }

            return null;
        }
    }
}
