using FieldSimulator.PowerSimulator.Calculations;
using System.Linq;
using FieldSimulator.PowerSimulator.Model.Measurements;

namespace FieldSimulator.PowerSimulator.Model.Equipment
{
    class EnergyStorage : ConductingEquipment
    {
        public EnergyStorage(long globalId) : base(globalId)
        {
        }

        public Generator Generator { get; set; }

        public float NominalPower { get; set; }

        public float Capacity { get; set; }

        public override Calculation CreateCalculation()
        {
            EnergyStorageStateOfChargeCalculation calculation = new EnergyStorageStateOfChargeCalculation(NominalPower, Capacity);

            PopulateActivePowerInputParameter(calculation);
            PopulateStateOfChargeOutputParameter(calculation);

            return calculation;
        }

        public override void Update(DERMS.IdentifiedObject cimObject)
        {
            base.Update(cimObject);

            DERMS.EnergyStorage cimEnergyStorage = cimObject as DERMS.EnergyStorage;

            if (cimEnergyStorage == null)
            {
                return;
            }

            NominalPower = cimEnergyStorage.NominalPower;
            Capacity = cimEnergyStorage.Capacity;
        }

        private void PopulateActivePowerInputParameter(Calculation calculation)
        {
            foreach (var analogMeasurement in Measurements.Cast<AnalogMeasurement>())
            {
                if (analogMeasurement.MeasurementType == MeasurementType.ActivePower)
                {
                    calculation.AddInputParameter(analogMeasurement.RemotePointType, analogMeasurement.Address);
                    return;
                }
            }          
        }

        private void PopulateStateOfChargeOutputParameter(EnergyStorageStateOfChargeCalculation calculation)
        {
            foreach (var analogMeasurement in Measurements.Cast<AnalogMeasurement>())
            {
                if (analogMeasurement.MeasurementType == MeasurementType.Percent)
                {
                    calculation.AddOutput(analogMeasurement.RemotePointType, analogMeasurement.Address);
                    return;
                }
            }
        }

    }
}
