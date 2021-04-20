using FieldSimulator.PowerSimulator.Calculations;
using FieldSimulator.PowerSimulator.Model.Measurements;
using System.Linq;

namespace FieldSimulator.PowerSimulator.Model.Equipment
{
    public class SolarPanel : Generator
    {
        public SolarPanel(long globalId) : base(globalId)
        {
        }

        public override Calculation CreateCalculation()
        {
            SolarPanelProductionCalculation calculation = new SolarPanelProductionCalculation(NominalPower);
            PopulateActivePowerOutputParameter(calculation);

            return calculation;
        }

        private void PopulateActivePowerOutputParameter(Calculation calculation)
        {
            foreach (var analogMeasurement in Measurements.Cast<AnalogMeasurement>())
            {
                if (analogMeasurement.MeasurementType == MeasurementType.ActivePower)
                {
                    calculation.AddOutput(analogMeasurement.RemotePointType, analogMeasurement.Address);
                    return;
                }
            }
        }
    }
}
